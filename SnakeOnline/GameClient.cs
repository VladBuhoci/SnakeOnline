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
        private int currentUniqueGameManagerID;
        private Queue<byte[]> rawDataBufferQueue = new Queue<byte[]>();

        private ClientMenuWindow clientMenuWindow;
        private ClientLobbyWindow clientLobbyWindow { get; set; }

        public SnakeGameManagerCL snakeGameManagerCL { get; set; }

        public GameClient(ClientMenuWindow menuWindow)
        {
            SetUpClientSocket();

            clientMenuWindow = menuWindow;
        }

        public void LoopConnect()
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
                    socket.Connect(IPAddress.Loopback, serverPortNumber);
                }
                catch (SocketException ex)
                {
                    //MessageBox.Show("Cannot connect to server :(");
                }
            }

            clientMenuWindow.ConnectionWasSuccessful();

            // Now that we're connected, we can send and receive messages from the server.
            WaitReceiveAndHandleDataFromServer();
        }

        private void WaitReceiveAndHandleDataFromServer()
        {
            byte[] rawDataBuffer = new byte[1024];

            // Begin receiving data from the server.
            socket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(BeginReceiveDataFromServer), socket);

            rawDataBufferQueue.Enqueue(rawDataBuffer);
        }
        
        private void BeginReceiveDataFromServer(IAsyncResult AR)
        {
            Socket socket = (Socket) AR.AsyncState;

            try
            {
                int receivedDataSize = socket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];
                
                Array.Copy(rawDataBufferQueue.Dequeue(), actualDataBuffer, receivedDataSize);

                // Resume receiving data from the server.
                byte[] rawDataBuffer = new byte[1024];

                socket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(BeginReceiveDataFromServer), socket);

                rawDataBufferQueue.Enqueue(rawDataBuffer);
                
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

                    switch (command)
                    {
                        case Socp.SEND_CLIENT_TEMP_UNIQUE_ID:
                            {
                                // Store the temporary id.
                                uniquePlayerID = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                                break;
                            }

                        case Socp.ACCEPT_NEW_CLIENT_WITH_NICKNAME:
                            {
                                string chosenNickname = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                                if (chosenNickname != "")
                                {
                                    uniquePlayerID = chosenNickname;

                                    ClientLobbyWindow lobbyWindow = new ClientLobbyWindow(this);

                                    this.clientLobbyWindow = lobbyWindow;
                                    this.clientMenuWindow.Visible = false;
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

                                    clientLobbyWindow.UpdateConnectedClientsList(names);
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

                        case Socp.GAME_ROOM_REQUEST_ACCEPTED:
                            {
                                SnakeGameDescriptor gameDescriptor = (SnakeGameDescriptor) SocpUtils.GetDataFromCommand(dataBuffer);
                                currentUniqueGameManagerID = gameDescriptor.gameManagerID;

                                Form gameWindow = new ClientGameWindow(this, clientLobbyWindow, currentUniqueGameManagerID);
                                gameWindow.ShowDialog(clientLobbyWindow);
                                
                                break;
                            }

                        case Socp.SEND_ARENA_MATRIX:
                            {
                                // TODO

                                break;
                            }

                        case Socp.RESPONSE_DISCONNECT_FROM_GAME_ROOM:
                            {
                                // TODO

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

        // TODO: useful?
        public void SendDataToServer(byte[] data)
        {
            socket.Send(data);
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

        public void SendChatMessageInLobby(string message)
        {
            socket.Send(SocpUtils.MakeNetworkCommand(Socp.CLIENT_POST_NEW_CHAT_MESSAGE_LOBBY, message, uniquePlayerID));
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
            // TODO
        }

        public void SendDisconnectFromServerRequest()
        {
            int gameManagerID = snakeGameManagerCL != null ? snakeGameManagerCL.GetUniqueGameManagerID() : -1;

            socket.Send(SocpUtils.MakeNetworkCommand(Socp.REQUEST_DISCONNECT_FROM_SERVER, "", uniquePlayerID, gameManagerID));
        }

        private void SetUpClientSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPortNumber = 1702;
            currentUniqueGameManagerID = -1;
        }        
    }
}
