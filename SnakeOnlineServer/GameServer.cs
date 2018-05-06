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
        private Dictionary<int, SnakeGameDescriptor> snakeGameRoomDescrCollection;  // TODO: is this even needed? we could just use the managers..

        private byte[] rawDataBuffer = new byte[1024];
        private int uniquePlayerTempIDCounter;
        private int uniqueGameManagerIDCounter;

        public GameServer(TextBox _serverLogTextBox)
        {
            serverLogTextBox             = _serverLogTextBox;

            serverSocket                 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tempIdClientSocketPairs      = new Dictionary<string, Socket>();
            idClientSocketPairs          = new Dictionary<string, Socket>();
            snakeGameManagerSVCollection = new Dictionary<int, SnakeGameManagerSV>();
            snakeGameRoomDescrCollection = new Dictionary<int, SnakeGameDescriptor>();
            rawDataBuffer                = new byte[1024];
            uniquePlayerTempIDCounter    = 0;
            uniqueGameManagerIDCounter   = 0;
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
           
            // TODO: should probably get rid of this.
            if (! clientSocket.Connected)
            {
                return;
            }

            try
            {
                int receivedDataSize = clientSocket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];
                
                Array.Copy(rawDataBuffer, actualDataBuffer, receivedDataSize);
                
                // Resume receiving data from this client socket.
                clientSocket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), clientSocket);
                
                // Handle the received data.
                HandleReceivedData(actualDataBuffer, clientSocket);
            }
            catch (SocketException ex)
            {
                LogMessage("\n~~~~~~~~~~~~~~~~~");
                LogMessage("~ Client disconnected unexpectedly!");
                LogMessage(ex.Message);
                LogMessage(ex.StackTrace);
                LogMessage("~~~~~~~~~~~~~~~~~\n");

                string key = null;
                var enumerator = idClientSocketPairs.GetEnumerator();

                // TODO: does this even work?
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
            }
        }
        
        private void HandleReceivedData(byte[] dataBuffer, Socket clientSocket)
        {
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
                                BroadcastClientListForLobby(chosenNickname);
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

                            LogMessage(String.Format("Client \"{0}\" requests a client list update for his/her lobby window.", clientID));

                            SendClientListForLobbyTo(clientID);

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
                            SnakeGameManagerSV gameManager = new SnakeGameManagerSV(gameDescriptor.arenaWidth, gameDescriptor.arenaHeight, gameDescriptor.foodEffect, gameDescriptor.matchDuration, this, playerID);

                            gameDescriptor.gameManagerID = uniqueGameManagerIDCounter;

                            // Store the game's manager and descriptor using the same ID.
                            snakeGameManagerSVCollection.Add(uniqueGameManagerIDCounter, gameManager);
                            snakeGameRoomDescrCollection.Add(uniqueGameManagerIDCounter, gameDescriptor);

                            // Send the result back to the client (the new room leader).
                            idClientSocketPairs[playerID].Send(SocpUtils.MakeNetworkCommand(Socp.GAME_ROOM_REQUEST_ACCEPTED, gameDescriptor));

                            // Send the collection of rooms to every client.
                            BroadcastRoomListForLobby();

                            // Increment the counter.
                            uniqueGameManagerIDCounter += 1;

                            break;
                        }

                    case Socp.REQUEST_GAME_ROOM_COLLECTION_UPDATE:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" requests a room list update for his/her lobby window.", clientID));

                            SendRoomListForLobbyTo(clientID);

                            break;
                        }

                        // TODO: needed anymore??
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
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" wants to disconnect from the current game room. (id: {1})", playerName, gameManagerID));

                            // TODO: get the player out of the game manager's collection and stuff, then
                            //          declare another player as the new room leader, if there is one,
                            //          or destroy the room.
                            {
                                // for now, just remove the room from the collection.
                                snakeGameManagerSVCollection[gameManagerID].RemovePlayerFromGame(playerName);

                                if (snakeGameManagerSVCollection[gameManagerID].IsGameEmpty())
                                {
                                    snakeGameManagerSVCollection.Remove(gameManagerID);
                                    snakeGameRoomDescrCollection.Remove(gameManagerID);
                                }
                            }

                            // Update the list of rooms for everyone.
                            BroadcastRoomListForLobby();

                            idClientSocketPairs[playerName].Send(SocpUtils.MakeNetworkCommand(Socp.RESPONSE_DISCONNECT_FROM_GAME_ROOM, ""));

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
                            BroadcastClientListForLobby();

                            break;
                        }

                    case Socp.PING:
                        {
                            string clientName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            string msgData = (string) SocpUtils.GetDataFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" pinged server. Additional message: {1}", clientName, msgData));

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

        private void SendClientListForLobbyTo(string clientID)
        {
            byte[] connectedClientsPacket = PrepareClientListForLobby();

            idClientSocketPairs[clientID].Send(connectedClientsPacket);
        }

        private void BroadcastClientListForLobby()
        {
            BroadcastClientListForLobby("");
        }

        private void BroadcastClientListForLobby(string exceptionClientID)
        {
            byte[] connectedClientsPacket = PrepareClientListForLobby();

            foreach (string id in idClientSocketPairs.Keys)
            {
                if (id != exceptionClientID)
                {
                    idClientSocketPairs[id].Send(connectedClientsPacket);
                }
            }
        }

        private byte[] PrepareClientListForLobby()
        {
            string[] clientsNames = idClientSocketPairs.Keys.ToArray();
            byte[] connectedClientsPacket = SocpUtils.MakeNetworkCommand(Socp.SEND_CONNECTED_CLIENTS_COLLECTION, clientsNames);

            return connectedClientsPacket;
        }

        private void SendRoomListForLobbyTo(string clientID)
        {
            byte[] roomsShortDescrCollectionPacket = PrepareRoomListForLobbyNetworkPacket();

            idClientSocketPairs[clientID].Send(roomsShortDescrCollectionPacket);
        }

        private void BroadcastRoomListForLobby()
        {
            byte[] roomsShortDescrCollectionPacket = PrepareRoomListForLobbyNetworkPacket();

            foreach (Socket socket in idClientSocketPairs.Values)
            {
                socket.Send(roomsShortDescrCollectionPacket);
            }
        }

        private byte[] PrepareRoomListForLobbyNetworkPacket()
        {
            List<SnakeGameShortDescriptor> snakeGameShortDescriptors = new List<SnakeGameShortDescriptor>();

            foreach (SnakeGameDescriptor gameDescriptor in snakeGameRoomDescrCollection.Values.ToArray())
            {
                SnakeGameShortDescriptor gameShortDescriptor = new SnakeGameShortDescriptor()
                {
                    roomName = gameDescriptor.roomName,
                    hasPassword = String.IsNullOrEmpty(gameDescriptor.roomPassword.Trim()) ? false : true,
                    currentPlayerCount = gameDescriptor.currentPlayerCount,
                    currentSpectatorCount = gameDescriptor.currentSpectatorCount,
                    roomState = gameDescriptor.roomState
                };

                snakeGameShortDescriptors.Add(gameShortDescriptor);
            }

            byte[] roomsShortDescrCollectionPacket = SocpUtils.MakeNetworkCommand(Socp.SERVER_BROADCAST_GAME_ROOM_COLLECTION, snakeGameShortDescriptors.ToArray());

            return roomsShortDescrCollectionPacket;
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
