using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnlineServer
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
                newClientSocket.Send(SocpUtils.MakeNetworkCommand(Socp.SEND_CLIENT_TEMP_UNIQUE_ID, uniquePlayerTempIDCounter.ToString()));
                
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
                //MessageBox.Show(null, "There was a problem while trying to accept a new client connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ServerBeginReceiveDataFromClient(IAsyncResult AR)
        {
            Socket clientSocket = (Socket) AR.AsyncState;
           
            if (! clientSocket.Connected)
            {
                return;
            }

            try
            {
                int receivedDataSize = clientSocket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];

                Array.Copy(rawDataBuffer, actualDataBuffer, receivedDataSize);

                // Handle the received data.
                HandleReceivedData(actualDataBuffer, clientSocket);
                
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
                Socp command = SocpUtils.GetProtocolValueFromCommand(dataBuffer);

                switch (command)
                {
                    case Socp.CONNECT_TO_LOBBY_WITH_NICNKNAME:
                        {
                            string tempID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            string chosenNickname = (string) SocpUtils.GetDataFromCommand(dataBuffer);

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

                                clientSocketWithNickname.Send(SocpUtils.MakeNetworkCommand(Socp.ACCEPT_NEW_CLIENT_WITH_NICKNAME, chosenNickname));

                                // Inform every client that a new user has connected to the server, by sending
                                //      a new collection of names to each client to be seen in their lobby.
                                BroadcastClientsListForLobby();
                            }
                            else
                            {
                                clientSocket.Send(SocpUtils.MakeNetworkCommand(Socp.ACCEPT_NEW_CLIENT_WITH_NICKNAME, ""));
                            }

                            break;
                        }

                    case Socp.REQUEST_LOBBY_PEOPLE_LIST_UPDATE:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            string[] clientsNames = idClientSocketPairs.Keys.ToArray();
                            byte[] connectedClientsCollection = SocpUtils.MakeNetworkCommand(Socp.SEND_CONNECTED_CLIENTS_COLLECTION, clientsNames);

                            idClientSocketPairs[clientID].Send(connectedClientsCollection);

                            break;
                        }

                    case Socp.CLIENT_POST_NEW_CHAT_MESSAGE_LOBBY:
                        {
                            string clientName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            string message = (string) SocpUtils.GetDataFromCommand(dataBuffer);
                            string finalChatMsg = String.Format("{0}: {1}", clientName, message);

                            // Send the chat message to every client.
                            foreach (Socket client in idClientSocketPairs.Values)
                            {
                                client.Send(SocpUtils.MakeNetworkCommand(Socp.SERVER_BROADCAST_NEW_CHAT_MESSAGE_LOBBY, finalChatMsg));
                            }

                            break;
                        }

                    case Socp.REQUEST_GAME_ROOM_CREATION:
                        {
                            SnakeGameDescriptor gameDescriptor = (SnakeGameDescriptor) SocpUtils.GetDataFromCommand(dataBuffer);
                            string playerID = gameDescriptor.roomLeaderID;

                            LogMessage(String.Format("New game room request from \"{0}.\"", playerID));

                            // Create the game manager here and store it in the coresponding collection.
                            SnakeGameManagerSV gameManager = new SnakeGameManagerSV(gameDescriptor.arenaWidth, gameDescriptor.arenaHeight, gameDescriptor.foodEffect, gameDescriptor.matchDuration, this);

                            snakeGameManagerSVCollection.Add(uniqueGameManagerIDCounter, gameManager);

                            gameDescriptor.gameManagerID = uniqueGameManagerIDCounter;
                            
                            // Send the result back to the client (the new room leader).
                            idClientSocketPairs[playerID].Send(SocpUtils.MakeNetworkCommand(Socp.GAME_ROOM_REQUEST_ACCEPTED, gameDescriptor));

                            // Increment the counter.
                            uniqueGameManagerIDCounter += 1;

                            break;
                        }

                    case Socp.SPAWN_SNAKE:
                        {
                            LogMessage("Player requested a snake to be created.");

                            // Create a snake and a unique ID and return the ID to the player.
                            // ... but not here. The right place is the game manager.
                            // ... also, it might be better to just generate an ID for each player when they connect for the first time.

                            break;
                        }

                    case Socp.REQUEST_DISCONNECT_FROM_GAME_ROOM:
                        {
                            string playerName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" wants to disconnect from the current game room.", playerName));

                            // TODO

                            break;
                        }

                    case Socp.REQUEST_DISCONNECT_FROM_SERVER:
                        {
                            string playerName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" wants to disconnect from the server.", playerName));

                            // If the player is connected to a game room at the moment, first attempt to get him/her
                            //      out of there and only then disconnect the client from the server.
                            if (gameManagerID > -1)
                            {
                                // TODO
                            }

                            byte[] response = SocpUtils.MakeNetworkCommand(Socp.RESPONSE_DISCONNECT_FROM_SERVER, "");

                            clientSocket.BeginSend(response, 0, response.Length, SocketFlags.None, new AsyncCallback(RequestDisconnectFromServerCallback), clientSocket);

                            // Remove it from the collection and close it.
                            if (tempIdClientSocketPairs.ContainsKey(playerName))
                                tempIdClientSocketPairs.Remove(playerName);
                            else
                                idClientSocketPairs.Remove(playerName);
                            
                            // Send an updated list of connected users to every single one of them.
                            BroadcastClientsListForLobby();

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

        private void BroadcastClientsListForLobby()
        {            
            string[] clientsNames = idClientSocketPairs.Keys.ToArray();
            byte[] connectedClientsCollection = SocpUtils.MakeNetworkCommand(Socp.SEND_CONNECTED_CLIENTS_COLLECTION, clientsNames);

            foreach (Socket socket in idClientSocketPairs.Values)
            {
                socket.Send(connectedClientsCollection);
            }
        }

        private void RequestDisconnectFromServerCallback(IAsyncResult AR)
        {
            Socket clientSocket = (Socket) AR.AsyncState;

            clientSocket.EndSend(AR);
            clientSocket.Shutdown(SocketShutdown.Send);
            clientSocket.Disconnect(false);
            clientSocket.Close();

            LogMessage("A client has disconnected from the server.");
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
