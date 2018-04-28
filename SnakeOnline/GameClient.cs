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
            if (CommunicationProtocolUtils.IsCommandNotEmpty(dataBuffer))
            {
                if (CommunicationProtocolUtils.GetPlayerIDFromCommand(dataBuffer) == "")
                {
                    CommunicationProtocol command = CommunicationProtocolUtils.GetProtocolValueFromCommand(dataBuffer);

                    switch (command)
                    {
                        case CommunicationProtocol.SEND_CLIENT_TEMP_UNIQUE_ID:
                            {
                                // Store the temporary id.
                                uniquePlayerID = (string) CommunicationProtocolUtils.GetDataFromCommand(dataBuffer);

                                break;
                            }

                        case CommunicationProtocol.ACCEPT_NEW_CLIENT_WITH_NICKNAME:
                            {
                                string chosenNickname = (string) CommunicationProtocolUtils.GetDataFromCommand(dataBuffer);

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

                        case CommunicationProtocol.SEND_CONNECTED_CLIENTS_COLLECTION:
                            {
                                if (clientLobbyWindow != null)
                                {
                                    string[] names = (string[]) CommunicationProtocolUtils.GetDataFromCommand(dataBuffer);

                                    clientLobbyWindow.UpdateConnectedClientsList(names);
                                }

                                break;
                            }

                        case CommunicationProtocol.SERVER_BROADCAST_NEW_CHAT_MESSAGE_LOBBY:
                            {
                                if (clientLobbyWindow != null)
                                {
                                    string message = (string) CommunicationProtocolUtils.GetDataFromCommand(dataBuffer);

                                    clientLobbyWindow.UpdateLobbyChat(message);
                                }

                                break;
                            }

                        case CommunicationProtocol.SEND_GAME_MANAGER_ID:
                            {
                                currentUniqueGameManagerID = CommunicationProtocolUtils.GetGameManagerIDFromCommand(dataBuffer);

                                Form gameWindow = new ClientGameWindow(this, clientLobbyWindow, currentUniqueGameManagerID);
                                clientLobbyWindow.Visible = false;
                                gameWindow.ShowDialog(clientLobbyWindow);

                                break;
                            }

                        case CommunicationProtocol.SEND_ARENA_MATRIX:
                            {
                                // TODO
                                break;
                            }
                    }
                }
            }
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
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, -1, CommunicationProtocol.CONNECT_TO_LOBBY_WITH_NICNKNAME, nickname));
        }

        public void SendUpdatedLobbyPeopleListRequest()
        {
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, -1, CommunicationProtocol.REQUEST_LOBBY_PEOPLE_LIST_UPDATE, ""));
        }

        public void SendChatMessageInLobby(string message)
        {
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, -1, CommunicationProtocol.CLIENT_POST_NEW_CHAT_MESSAGE_LOBBY, message));
        }

        public void SendCreateGameRequestToServer()
        {
            // TODO: add every client id in the data field?
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, -1, CommunicationProtocol.CREATE_GAME, "NODATA YET | probably many client IDs"));
        }

        public void SendSnakeSpawnRequestToServer()
        {
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID(), CommunicationProtocol.SPAWN_SNAKE, "add snake properties here"));
        }

        private void SetUpClientSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPortNumber = 1702;
            currentUniqueGameManagerID = -1;
        }

        /// <summary>
        ///     Clean up method for when the client disconnects from the server.
        /// </summary>
        public void DisconnectFromServer()
        {
            if (socket != null)
            {
                socket.Disconnect(true);
                socket = null;
            }
        }

        /// <summary>
        ///     Final clean up method used when the program closes.
        /// </summary>
        public void CleanUp()
        {
            if (socket != null)
            {
                socket.Close();
                socket.Dispose();
                socket = null;

                if (snakeGameManagerCL != null)
                {
                    snakeGameManagerCL.RequestAuxGameLoopThreadToEnd();
                    snakeGameManagerCL = null;
                }
            }
        }
    }
}
