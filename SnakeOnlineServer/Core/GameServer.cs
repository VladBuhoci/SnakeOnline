using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnlineServer.Core
{
    class GameServer
    {
        private TextBox serverLogTextBox;

        private Socket serverSocket;
        private Dictionary<string, Socket> tempIdClientSocketPairs;
        private Dictionary<string, Socket> idClientSocketPairs;
        private Dictionary<int, SnakeGameManagerSV> snakeGameManagerSVCollection;

        private byte[] rawDataBuffer = new byte[1024];  // TODO: needed as a class field?
        private int uniquePlayerTempIDCounter;
        private int uniqueGameManagerIDCounter;

        public GameServer(TextBox _serverLogTextBox)
        {
            serverLogTextBox             = _serverLogTextBox;
            snakeGameManagerSVCollection = new Dictionary<int, SnakeGameManagerSV>();

            serverSocket                 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tempIdClientSocketPairs      = new Dictionary<string, Socket>();
            idClientSocketPairs          = new Dictionary<string, Socket>();
            uniquePlayerTempIDCounter    = 0;
            uniqueGameManagerIDCounter   = 0;

            //CreateNewGameAndGameManager();      // TODO: shouldn't be done here.
        }

        public void SetUpServer()
        {
            LogMessage("Attempting to start server...");

            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1702));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(ServerAcceptConnectionCallback), null);

            LogMessage("Server has started.");
        }

        private void CreateNewGameAndGameManager(int arenaWidth, int arenaHeight, Dictionary<int, Snake> idSnakePairs)
        {
            SnakeGameManagerSV manager = new SnakeGameManagerSV(arenaWidth, arenaHeight, idSnakePairs);

            snakeGameManagerSVCollection.Add(uniqueGameManagerIDCounter, manager);

            uniqueGameManagerIDCounter += 1;
        }

        private void ServerAcceptConnectionCallback(IAsyncResult AR)
        {
            try
            {
                Socket newClientSocket = serverSocket.EndAccept(AR);

                // Add this socket to the temporary collection of clients.
                tempIdClientSocketPairs.Add(uniquePlayerTempIDCounter.ToString(), newClientSocket);

                // Let the client know of its temporary ID.
                // The client will later send this back along with their chosen nickname, which replaces
                //      the temporary identifier.
                newClientSocket.Send(CommunicationProtocolUtils.MakeNetworkCommand(null, -1, CommunicationProtocol.SEND_CLIENT_TEMP_UNIQUE_ID, uniquePlayerTempIDCounter.ToString()));
                
                LogMessage(String.Format("New client has connected to the server. (temporary id: {0})", uniquePlayerTempIDCounter));

                // Increment the counter.
                uniquePlayerTempIDCounter += 1;

                // Begin receiving data from this client.
                newClientSocket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), newClientSocket);
                
                // Resume accepting connections.
                serverSocket.BeginAccept(new AsyncCallback(ServerAcceptConnectionCallback), null);
            }
            catch
            {
                //MessageBox.Show(null, "There was a problem in trying to accept a new client connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ServerBeginReceiveDataFromClient(IAsyncResult AR)
        {
            Socket clientSocket = (Socket) AR.AsyncState;

            try
            {
                int receivedDataSize = clientSocket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];

                Array.Copy(rawDataBuffer, actualDataBuffer, receivedDataSize);

                // Handle the received data.
                HandleReceivedData(actualDataBuffer, clientSocket);
                
                // TODO #1: after handling the data, send things back to the client(s).
                // TODO #2: is this really where (and how) to do it?
                //SendDataToClient();

                // Resume receiving data from this client socket.
                clientSocket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), clientSocket);
            }
            catch (Exception e)
            {
                LogMessage("~ Client disconnected.");
                LogMessage(e.Message);
                LogMessage(e.StackTrace);

                string key = null;
                var enumerator = idClientSocketPairs.GetEnumerator();

                while (enumerator.Current.Value != null)
                {
                    if (enumerator.Current.Value == clientSocket)
                    {
                        key = enumerator.Current.Key;
                        break;
                    }

                    enumerator.MoveNext();
                }

                if (key != null)
                {
                    idClientSocketPairs.Remove(key);
                }

                clientSocket.Disconnect(true);
                clientSocket = null;
            }
        }

        private void HandleReceivedData(byte[] dataBuffer, Socket clientSocket)
        {
            // TODO
            //if (CommunicationProtocolUtils.IsCommandNotEmpty(dataBuffer))
            {
                CommunicationProtocol command = CommunicationProtocolUtils.GetProtocolValueFromCommand(dataBuffer);

                switch (command)
                {
                    case CommunicationProtocol.CONNECT_TO_LOBBY_WITH_NICNKNAME:
                        {
                            string tempID = CommunicationProtocolUtils.GetPlayerIDFromCommand(dataBuffer);
                            string chosenNickname = (string) CommunicationProtocolUtils.GetDataFromCommand(dataBuffer);

                            // Make sure this new name is not already in use by another player.
                            // If it is, we will let the player know about this.

                            bool isPlayerAllowedToJoinLobby = ! idClientSocketPairs.Keys.Contains(chosenNickname);

                            if (isPlayerAllowedToJoinLobby)
                            {
                                // Replace old ID with nickname by removing and re-adding the entry.
                                // It's a work-around when trying to replace keys in dictionary objects.

                                Socket clientSocketWithNickname = tempIdClientSocketPairs.Where(s => s.Key == tempID).First().Value;

                                tempIdClientSocketPairs.Remove(tempID);
                                idClientSocketPairs.Add(chosenNickname, clientSocketWithNickname);

                                LogMessage(String.Format("Client \"" + chosenNickname + "\" (previous temporary id: {0}) has joined the lobby.", tempID));

                                clientSocketWithNickname.Send(CommunicationProtocolUtils.MakeNetworkCommand(null, -1, CommunicationProtocol.ACCEPT_NEW_CLIENT_WITH_NICKNAME, chosenNickname));

                                // Inform every client that a new user has connected to the server, by sending
                                //      a new collection of names to each client to be seen in their lobby.
                                string[] clientsNames = idClientSocketPairs.Keys.ToArray();
                                byte[] connectedClientsCollection = CommunicationProtocolUtils.MakeNetworkCommand(null, -1, CommunicationProtocol.SEND_CONNECTED_CLIENTS_COLLECTION, clientsNames);

                                foreach (Socket socket in idClientSocketPairs.Values)
                                {
                                    socket.Send(connectedClientsCollection);
                                }
                            }
                            else
                            {
                                clientSocket.Send(CommunicationProtocolUtils.MakeNetworkCommand(null, -1, CommunicationProtocol.ACCEPT_NEW_CLIENT_WITH_NICKNAME, ""));
                            }

                            break;
                        }

                    case CommunicationProtocol.REQUEST_LOBBY_PEOPLE_LIST_UPDATE:
                        {
                            string clientID = CommunicationProtocolUtils.GetPlayerIDFromCommand(dataBuffer);
                            string[] clientsNames = idClientSocketPairs.Keys.ToArray();
                            byte[] connectedClientsCollection = CommunicationProtocolUtils.MakeNetworkCommand(null, -1, CommunicationProtocol.SEND_CONNECTED_CLIENTS_COLLECTION, clientsNames);

                            idClientSocketPairs[clientID].Send(connectedClientsCollection);

                            break;
                        }

                    case CommunicationProtocol.CREATE_GAME:
                        {
                            LogMessage("New game request.");

                            string playerID = CommunicationProtocolUtils.GetPlayerIDFromCommand(dataBuffer);

                            // Create the game manager here and do thingies for it.
                            // ... also add it to the dictionary.
                            // ... NOTE: the command wrapper should contain every client ID that will participate in this match in the data field.

                            // foreach (clientID in list of IDs)
                            // Think of the data that could be sent here...
                            byte[] newManagerResultCommand = CommunicationProtocolUtils.MakeNetworkCommand(null, uniqueGameManagerIDCounter, CommunicationProtocol.SEND_GAME_MANAGER_ID, playerID);

                            // Send the result back.
                            idClientSocketPairs[playerID].Send(newManagerResultCommand);

                            //snakeGameManagerSVCollection.Add(uniqueGameManagerIDCounter, newManager);

                            uniqueGameManagerIDCounter += 1;

                            break;
                        }

                    case CommunicationProtocol.SPAWN_SNAKE:
                        {
                            LogMessage("Player requested a snake to be created.");

                            // Create a snake and a unique ID and return the ID to the player.
                            // ... but not here. The right place is the game manager.
                            // ... also, it might be better to just generate an ID for each player when they connect for the first time.

                            break;
                        }

                    default:
                        {
                            LogMessage("Unknown command received from client.");
                            break;
                        }
                }
            }
        }

        private void SendDataToClient()
        {
            //byte[] dataToSendBackToClient = ...
            //clientSocket.BeginSend(dataToSendBackToClient, 0, dataToSendBackToClient.Length, SocketFlags.None, new AsyncCallback(ServerSendToClientCallback), clientSocket);
        }

        private void ServerSendToClientCallback(IAsyncResult AR)
        {
            Socket clientSocket = (Socket) AR.AsyncState;

            clientSocket.EndSend(AR);
        }

        private void LogMessage(String text)
        {
            if (serverLogTextBox != null)
            {
                serverLogTextBox.BeginInvoke(new MethodInvoker(delegate { serverLogTextBox.AppendText(text + "\n"); }));
            }
        }

        /// <summary>
        ///     Clean up method.
        /// </summary>
        public void CleanUp()
        {
            serverLogTextBox.Clear();

            // close all sockets
            foreach (Socket clientSocket in tempIdClientSocketPairs.Values)
            {
                clientSocket.Close();
                clientSocket.Dispose();
            }

            foreach (Socket clientSocket in idClientSocketPairs.Values)
            {
                clientSocket.Close();
                clientSocket.Dispose();
            }

            tempIdClientSocketPairs.Clear();
            idClientSocketPairs.Clear();

            serverSocket.Close();
            serverSocket.Dispose();
            serverSocket = null;

            foreach (SnakeGameManagerSV manager in snakeGameManagerSVCollection.Values)
            {
                manager.RequestAuxGameLoopThreadToEnd();
            }
        }
    }
}
