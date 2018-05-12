﻿#define IS_DEBUG

using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline
{
    class GameClient
    {
        private Socket socket;

        private int serverPortNumber;
        private string uniquePlayerID;
        private byte[] rawDataBuffer = new byte[1024];

        private ClientMenuWindow clientMenuWindow;
        private ClientLobbyWindow clientLobbyWindow;
        private ClientGameWindow clientGameWindow;

        public SnakeGameManagerCL snakeGameManagerCL { get; set; }

        public GameClient(ClientMenuWindow menuWindow)
        {
            SetUpClientSocket();

            clientMenuWindow = menuWindow;
        }

        public void LoopConnect(string ipAddress)
        {
            int attempts = 0;

            if (socket == null)
            {
                SetUpClientSocket();
            }

            while (! socket.Connected)
            {
                // TODO: find some usage for this.
                attempts += 1;

                try
                {
                    socket.Connect(IPAddress.Parse(ipAddress), serverPortNumber);
                }
                catch (SocketException ex)
                {
                    //MessageBox.Show("Cannot connect to server :(");
                }
            }

            // Now that we're connected, we can send and receive messages from the server.
            WaitReceiveAndHandleDataFromServer();
        }

        private void WaitReceiveAndHandleDataFromServer()
        {
            // Begin receiving data from the server.
            socket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(BeginReceiveDataFromServer), socket);
        }
        
        private void BeginReceiveDataFromServer(IAsyncResult AR)
        {
            Socket socket = (Socket) AR.AsyncState;

            try
            {
                int receivedDataSize = socket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];
                
                Array.Copy(rawDataBuffer, actualDataBuffer, receivedDataSize);
                
                // Resume receiving data from the server.
                socket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(BeginReceiveDataFromServer), socket);
                
                // Deal with the received data.
                HandleReceivedData(actualDataBuffer);
            }
            catch (SocketException ex)
            {
                socket.Disconnect(true);
                socket = null;
            }
        }

        private void HandleReceivedData(byte[] dataBuffer)
        {
            if (SocpUtils.IsCommandNotEmpty(dataBuffer))
            {
                if (SocpUtils.GetPlayerIDFromCommand(dataBuffer) == "")
                {
                    Socp command = SocpUtils.GetProtocolValueFromCommand(dataBuffer);

#if IS_DEBU
                    // Testing purposes only.
                    SendPingToServer(command.ToString());
#endif //IS_DEBUG

                    switch (command)
                    {
                        case Socp.SEND_CLIENT_TEMP_UNIQUE_ID:
                            {
                                // Store the temporary id.
                                uniquePlayerID = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                                clientMenuWindow.PromptClientForUsername();

                                break;
                            }

                        case Socp.ACCEPT_NEW_CLIENT_WITH_NICKNAME:
                            {
                                string chosenNickname = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                                if (chosenNickname != "")
                                {
                                    uniquePlayerID = chosenNickname;

                                    ClientLobbyWindow lobbyWindow = new ClientLobbyWindow(this);
                                    
                                    this.clientMenuWindow.Hide();

                                    this.clientLobbyWindow = lobbyWindow;
                                    this.clientLobbyWindow.ShowDialog(clientMenuWindow);
                                }
                                else
                                {
                                    MessageBox.Show(clientMenuWindow, "Name already used.", "Nickname taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                break;
                            }

                        case Socp.SEND_CONNECTED_CLIENTS_COLLECTION:
                            {
                                if (clientLobbyWindow != null)
                                {
                                    string[] names = (string[]) SocpUtils.GetDataFromCommand(dataBuffer);

                                    clientLobbyWindow.UpdateLobbyConnectedClientsList(names);
                                }

                                break;
                            }

                        case Socp.SEND_ROOM_PLAYER_COLLECTION:
                            {
                                if (clientGameWindow != null)
                                {
                                    string[] names = (string[]) SocpUtils.GetDataFromCommand(dataBuffer);

                                    clientGameWindow.UpdateRoomPlayerList(names);
                                }

                                break;
                            }

                        case Socp.SEND_ROOM_SPECTATOR_COLLECTION:
                            {
                                if (clientGameWindow != null)
                                {
                                    string[] names = (string[]) SocpUtils.GetDataFromCommand(dataBuffer);

                                    clientGameWindow.UpdateRoomSpectatorList(names);
                                }

                                break;
                            }

                        case Socp.SERVER_BROADCAST_NEW_CHAT_MESSAGE_LOBBY:
                            {
                                if (clientLobbyWindow != null)
                                {
                                    string message = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                                    clientLobbyWindow.UpdateLobbyChat(message);
                                }

                                break;
                            }

                        case Socp.SERVER_BROADCAST_NEW_CHAT_MESSAGE_ROOM:
                            {
                                if (clientLobbyWindow != null)
                                {
                                    string message = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                                    clientGameWindow.UpdateRoomChat(message);
                                }

                                break;
                            }

                        case Socp.GAME_ROOM_REQUEST_ACCEPTED:
                            {
                                SnakeGameDescriptor gameDescriptor = (SnakeGameDescriptor) SocpUtils.GetDataFromCommand(dataBuffer);
                                int currentUniqueGameManagerID = gameDescriptor.gameManagerID;

                                ClientGameWindow gameWindow = new ClientGameWindow(this, clientLobbyWindow, currentUniqueGameManagerID);

                                clientGameWindow = gameWindow;
                                clientGameWindow.ShowDialog(clientLobbyWindow);

                                break;
                            }

                        case Socp.SERVER_BROADCAST_GAME_ROOM_COLLECTION:
                            {
                                if (clientLobbyWindow != null)
                                {
                                    SnakeGameShortDescriptor[] rooms = (SnakeGameShortDescriptor[]) SocpUtils.GetDataFromCommand(dataBuffer);

                                    clientLobbyWindow.UpdateLobbyRoomList(rooms);
                                }

                                break;
                            }

                        case Socp.SEND_ARENA_MATRIX:
                            {
                                // TODO

                                break;
                            }

                        case Socp.RESPONSE_DISCONNECT_FROM_GAME_ROOM:
                            {
                                snakeGameManagerCL.RequestAuxGameLoopThreadToEnd();
                                snakeGameManagerCL = null;

                                // TODO: Anything to handle here?

                                break;
                            }

                        case Socp.RESPONSE_DISCONNECT_FROM_SERVER:
                            {
                                Application.Exit();

                                break;
                            }
                    }
                }
            }
        }

        public string GetUniquePlayerID()
        {
            return uniquePlayerID;
        }

        #region Methods handling sending data to server for different situations.

        public void SendPingToServer(string additionalMessage)
        {
            string msgToSend = String.IsNullOrEmpty(additionalMessage.Trim()) ? "" : additionalMessage.Trim();
            int gameManagerID = snakeGameManagerCL != null ? snakeGameManagerCL.GetUniqueGameManagerID() : -1;

            socket.Send(SocpUtils.MakeNetworkCommand(Socp.PING, msgToSend, uniquePlayerID, gameManagerID));
        }

        public void SendJoinLobbyRequestToServer(string nickname)
        {
            // Currently, uniquePlayerID is the temporary identifier sent by the server.
            // The server will replace it with the chosen nickname.
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.CONNECT_TO_LOBBY_WITH_NICNKNAME, nickname, uniquePlayerID));
        }

        public void SendUpdatedLobbyPeopleListRequest()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_LOBBY_PEOPLE_LIST_UPDATE, "", uniquePlayerID));
        }

        public void SendUpdatedRoomPlayerListRequest()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_ROOM_PLAYER_LIST_UPDATE, "", uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID()));
        }

        public void SendUpdatedRoomSpectatorListRequest()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_ROOM_SPECTATOR_LIST_UPDATE, "", uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID()));
        }

        public void SendPlayerSwitchSidesInRoomRequest()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_SWITCH_SIDES_ROOM, "", uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID()));
        }

        public void SendUpdatedLobbyRoomListRequest()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_GAME_ROOM_COLLECTION_UPDATE, "", uniquePlayerID));
        }

        public void SendChatMessageInLobby(string message)
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.CLIENT_POST_NEW_CHAT_MESSAGE_LOBBY, message, uniquePlayerID));
        }

        public void SendChatMessageInRoom(string message)
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.CLIENT_POST_NEW_CHAT_MESSAGE_ROOM, message, uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID()));
        }

        public void SendCreateGameRequestToServer(SnakeGameDescriptor descriptor)
        {
            // TODO: add every client id in the data field? (probably not.. this will happen at game start)
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_GAME_ROOM_CREATION, descriptor, uniquePlayerID));
        }

        public void SendSnakeSpawnRequestToServer()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.SPAWN_SNAKE, "add snake properties here", uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID()));
        }

        public void SendDisconnectFromGameRoomRequest()
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_DISCONNECT_FROM_GAME_ROOM, "", uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID()));
        }

        public void SendDisconnectFromServerRequest()
        {
            int gameManagerID = snakeGameManagerCL != null ? snakeGameManagerCL.GetUniqueGameManagerID() : -1;

            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_DISCONNECT_FROM_SERVER, "", uniquePlayerID, gameManagerID));
        }

        #endregion

        private void SetUpClientSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPortNumber = 1702;
        }
    }
}