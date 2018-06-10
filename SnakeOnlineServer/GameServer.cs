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

        private int serverPortNumber;
        private Socket serverSocket;
        private Dictionary<string, Socket> tempIdClientSocketPairs;
        private Dictionary<string, Socket> idClientSocketPairs;
        private Dictionary<int, SnakeGameManagerSV> snakeGameManagerSVCollection;
        
        private int uniquePlayerTempIDCounter;
        private int uniqueGameManagerIDCounter;

        public GameServer(TextBox _serverLogTextBox)
        {
            serverLogTextBox             = _serverLogTextBox;

            serverPortNumber             = 1702;
            serverSocket                 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tempIdClientSocketPairs      = new Dictionary<string, Socket>();
            idClientSocketPairs          = new Dictionary<string, Socket>();
            snakeGameManagerSVCollection = new Dictionary<int, SnakeGameManagerSV>();
            uniquePlayerTempIDCounter    = 0;
            uniqueGameManagerIDCounter   = 0;
        }

        public void SetUpServer()
        {
            LogMessage("Attempting to start server...");

            serverSocket.Bind(new IPEndPoint(IPAddress.Any, serverPortNumber));
            serverSocket.Listen(100);
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

                AsyncStateContainer nextStateContainer = new AsyncStateContainer(newClientSocket);

                // Begin receiving data from this client.
                nextStateContainer.socket.BeginReceive(nextStateContainer.dataBuffer, 0, nextStateContainer.dataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), nextStateContainer);
                
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
            AsyncStateContainer stateContainer = (AsyncStateContainer) AR.AsyncState;
            
            try
            {
                int receivedDataSize = stateContainer.socket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];
                
                Array.Copy(stateContainer.dataBuffer, actualDataBuffer, receivedDataSize);

                AsyncStateContainer nextStateContainer = new AsyncStateContainer(stateContainer.socket);

                // Resume receiving data from this client socket.
                nextStateContainer.socket.BeginReceive(nextStateContainer.dataBuffer, 0, nextStateContainer.dataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), nextStateContainer);
                
                // Handle the received data.
                HandleReceivedData(actualDataBuffer, stateContainer.socket);
            }
            catch (Exception se)
            {
                LogMessage("\n~~~~~~~~~~~~~~~~~");
                LogMessage("~ Client disconnected unexpectedly!");
                LogMessage(se.Message);
                LogMessage(se.StackTrace);
                LogMessage("~~~~~~~~~~~~~~~~~\n");

                string key = null;
                var enumerator = idClientSocketPairs.GetEnumerator();
                enumerator.MoveNext();
                
                while (enumerator.Current.Value != null)
                {
                    if (enumerator.Current.Value == stateContainer.socket)
                    {
                        key = enumerator.Current.Key;
                        break;
                    }

                    enumerator.MoveNext();
                }

                if (key != null)
                {
                    idClientSocketPairs[key].Shutdown(SocketShutdown.Send);
                    idClientSocketPairs[key].Disconnect(false);
                    idClientSocketPairs[key].Close();

                    idClientSocketPairs.Remove(key);
                }
            }
        }
        
        private void HandleReceivedData(byte[] dataBuffer, Socket clientSocket)
        {
            if (SocpUtils.IsCommandNotEmpty(dataBuffer))
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

                    case Socp.REQUEST_ROOM_PLAYER_LIST_UPDATE:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);
                            
                            BroadcastPlayerListForRoom(gameManagerID);

                            break;
                        }

                    case Socp.REQUEST_ROOM_SPECTATOR_LIST_UPDATE:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);
                            
                            BroadcastSpectatorListForRoom(gameManagerID);

                            break;
                        }

                    case Socp.REQUEST_SWITCH_SIDES_ROOM:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);

                            snakeGameManagerSVCollection[gameManagerID].SwitchSidesForClient(clientID);

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

                    case Socp.CLIENT_POST_NEW_CHAT_MESSAGE_ROOM:
                        {
                            string clientName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);
                            string message = (string) SocpUtils.GetDataFromCommand(dataBuffer);
                            string allegiance = snakeGameManagerSVCollection[gameManagerID].Players.Contains(clientName) ? "SNAK" : "SPEC";
                            string finalChatMsg = String.Format("[{0}] {1}: {2}", allegiance, clientName, message);

                            string[] roomClients = snakeGameManagerSVCollection[gameManagerID].Players.Concat(snakeGameManagerSVCollection[gameManagerID].Spectators).ToArray();

                            // Send the chat message to every client in that room.
                            foreach (string name in roomClients)
                            {
                                idClientSocketPairs[name].Send(SocpUtils.MakeNetworkCommand(Socp.SERVER_BROADCAST_NEW_CHAT_MESSAGE_ROOM, finalChatMsg));
                            }

                            break;
                        }

                    case Socp.REQUEST_GAME_ROOM_CREATION:
                        {
                            SnakeGameDescriptor gameDescriptor = (SnakeGameDescriptor) SocpUtils.GetDataFromCommand(dataBuffer);
                            string playerID = gameDescriptor.roomLeaderID;

                            LogMessage(String.Format("New game room request from \"{0}\".", playerID));

                            // Create the game manager here and store it in the coresponding collection.

                            gameDescriptor.gameManagerID = uniqueGameManagerIDCounter;

                            SnakeGameManagerSV gameManager = new SnakeGameManagerSV(this/*, uniqueGameManagerIDCounter*/, gameDescriptor);

                            // Store the game's manager using the ID.
                            snakeGameManagerSVCollection.Add(uniqueGameManagerIDCounter, gameManager);

                            // Send the result back to the client (the new room leader).
                            idClientSocketPairs[playerID].Send(SocpUtils.MakeNetworkCommand(Socp.GAME_ROOM_CREATION_REQUEST_ACCEPTED, gameDescriptor));

                            // Send the collection of rooms to every client.
                            BroadcastRoomListForLobby();

                            // Increment the counter.
                            uniqueGameManagerIDCounter += 1;

                            break;
                        }

                    case Socp.REQUEST_GAME_ROOM_DESCRIPTION:
                        {
                            string playerID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int roomID = (int) SocpUtils.GetDataFromCommand(dataBuffer);

                            LogMessage(String.Format("Player \"{0}\" wants info about room with id: {1}.", playerID, roomID));

                            idClientSocketPairs[playerID].Send(SocpUtils.MakeNetworkCommand(Socp.SEND_GAME_ROOM_DESCRIPTION, snakeGameManagerSVCollection[roomID].GetDescriptor()));

                            break;
                        }

                    case Socp.REQUEST_GAME_ROOM_JOIN:
                        {
                            string playerID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int roomID = (int) SocpUtils.GetDataFromCommand(dataBuffer);

                            LogMessage(String.Format("Player \"{0}\" joins room with id: {1}.", playerID, roomID));

                            snakeGameManagerSVCollection[roomID].AddClientToGameRoom(playerID);

                            idClientSocketPairs[playerID].Send(SocpUtils.MakeNetworkCommand(Socp.GAME_ROOM_JOIN_REQUEST_ACCEPTED, snakeGameManagerSVCollection[roomID].GetDescriptor()));

                            BroadcastRoomListForLobby();

                            break;
                        }

                    case Socp.REQUEST_GAME_ROOM_COLLECTION_UPDATE:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" requests a room list update for his/her lobby window.", clientID));

                            SendRoomListForLobbyTo(clientID);

                            break;
                        }
                        
                    case Socp.REQUEST_GAME_ROOM_MATCH_START:
                        {
                            string clientID = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Room leader \"{0}\" requested a match to begin in room {1}.", clientID, gameManagerID));

                            snakeGameManagerSVCollection[gameManagerID].StartGame();
                            
                            BroadcastRoomListForLobby();

                            break;
                        }

                    case Socp.REQUEST_CHANGE_SNAKE_ORIENTATION:
                        {
                            string playerName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);
                            SnakeOrientation newOrientation = (SnakeOrientation) SocpUtils.GetDataFromCommand(dataBuffer);

                            snakeGameManagerSVCollection[gameManagerID].ChangeSnakeOrientation(playerName, newOrientation);

                            break;
                        }

                    case Socp.REQUEST_DISCONNECT_FROM_GAME_ROOM:
                        {
                            string playerName = SocpUtils.GetPlayerIDFromCommand(dataBuffer);
                            int gameManagerID = SocpUtils.GetGameManagerIDFromCommand(dataBuffer);

                            LogMessage(String.Format("Client \"{0}\" wants to disconnect from the current game room. (id: {1})", playerName, gameManagerID));

                            // Remove the room from the collection.
                            snakeGameManagerSVCollection[gameManagerID].RemoveClientFromGameRoom(playerName);
                            
                            if (snakeGameManagerSVCollection[gameManagerID].IsGameEmpty())
                            {
                                snakeGameManagerSVCollection.Remove(gameManagerID);
                            }

                            idClientSocketPairs[playerName].Send(SocpUtils.MakeNetworkCommand(Socp.RESPONSE_DISCONNECT_FROM_GAME_ROOM, ""));

                            // Update the list of rooms for everyone.
                            BroadcastRoomListForLobby();

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

                            AttemptToDisconnectClientFromServer(clientSocket, playerName);
                            
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

        private void AttemptToDisconnectClientFromServer(Socket clientSocket, string playerName)
        {
            byte[] response = SocpUtils.MakeNetworkCommand(Socp.RESPONSE_DISCONNECT_FROM_SERVER, "");

            clientSocket.BeginSend(response, 0, response.Length, SocketFlags.None, new AsyncCallback(RequestDisconnectFromServerCallback), clientSocket);

            // Remove it from the collection and close it.
            if (tempIdClientSocketPairs.ContainsKey(playerName))
                tempIdClientSocketPairs.Remove(playerName);
            else
                idClientSocketPairs.Remove(playerName);
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

        #region Network utils.

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

        public void BroadcastPlayerListForRoom(int gameManagerID)
        {
            BroadcastPlayerListForRoom(gameManagerID, "");
        }

        private void BroadcastPlayerListForRoom(int gameManagerID, string exceptionClientID)
        {
            if (snakeGameManagerSVCollection.Keys.Contains(gameManagerID))
            {
                string[] playerNames = snakeGameManagerSVCollection[gameManagerID].Players;

                foreach (string id in idClientSocketPairs.Keys)
                {
                    if (id != exceptionClientID)
                    {
                        idClientSocketPairs[id].Send(SocpUtils.MakeNetworkCommand(Socp.SEND_ROOM_PLAYER_COLLECTION, playerNames));
                    }
                }
            }
        }

        public void BroadcastSpectatorListForRoom(int gameManagerID)
        {
            BroadcastSpectatorListForRoom(gameManagerID, "");
        }

        private void BroadcastSpectatorListForRoom(int gameManagerID, string exceptionClientID)
        {
            if (snakeGameManagerSVCollection.Keys.Contains(gameManagerID))
            {
                string[] spectatorNames = snakeGameManagerSVCollection[gameManagerID].Spectators;

                foreach (string id in idClientSocketPairs.Keys)
                {
                    if (id != exceptionClientID)
                    {
                        idClientSocketPairs[id].Send(SocpUtils.MakeNetworkCommand(Socp.SEND_ROOM_SPECTATOR_COLLECTION, spectatorNames));
                    }
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

        public void BroadcastRoomListForLobby()
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
            
            foreach (SnakeGameManagerSV gameManager in snakeGameManagerSVCollection.Values)
            {
                SnakeGameDescriptor gameDescriptor = gameManager.GetDescriptor();
                SnakeGameShortDescriptor gameShortDescriptor = new SnakeGameShortDescriptor()
                {
                    gameManagerID = gameDescriptor.gameManagerID,
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

        public void SendGameRoomLeaderHasChangedMessageToClients(int gameManagerID, string newLeaderID)
        {
            LogMessage("New leader: " + newLeaderID + " for room with id: " + gameManagerID + ".");

            byte[] roomLeaderHasChangedMsg = SocpUtils.MakeNetworkCommand(Socp.GAME_ROOM_LEADER_HAS_CHANGED, newLeaderID);

            foreach (string player in snakeGameManagerSVCollection[gameManagerID].Players)
            {
                idClientSocketPairs[player].Send(roomLeaderHasChangedMsg);
            }

            foreach (string spectator in snakeGameManagerSVCollection[gameManagerID].Spectators)
            {
                idClientSocketPairs[spectator].Send(roomLeaderHasChangedMsg);
            }

            LogMessage("Done with leader stuff.");
        }

        public void SendStartGameRequestToClient(string clientID, SnakeOrientation? snakeOrientation)
        {
            idClientSocketPairs[clientID].Send(SocpUtils.MakeNetworkCommand(Socp.RESPONSE_MATCH_STARTED, snakeOrientation));
        }

        public void SendUpdatedArenaDataToClients(int gameManagerID, SnakeGameArenaObject[, ] data, int timeLeft)
        {
            Dictionary<string, object> dataToSend = new Dictionary<string, object>();

            dataToSend.Add("arenaData", data);
            dataToSend.Add("timeLeft", timeLeft);

            byte[] updatedDataMsg = SocpUtils.MakeNetworkCommand(Socp.SEND_ARENA_DATA, dataToSend);

            foreach (string client in snakeGameManagerSVCollection[gameManagerID].AllClients)
            {
                idClientSocketPairs[client].Send(updatedDataMsg);
            }
        }

        public void SendGameOverResultToClients(int gameManagerID, string resultMsg, string leaderID)
        {
            LogMessage(String.Format("Game over in room with id: {0}.", gameManagerID));

            Dictionary<string, string> dataToSend = new Dictionary<string, string>();

            dataToSend.Add("resultMsg", resultMsg);
            dataToSend.Add("leaderID", leaderID);

            byte[] gameOverResultMsg = SocpUtils.MakeNetworkCommand(Socp.SEND_GAME_OVER_RESULT, dataToSend);

            foreach (string client in snakeGameManagerSVCollection[gameManagerID].AllClients)
            {
                idClientSocketPairs[client].Send(gameOverResultMsg);
            }
        }

        #endregion

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
            foreach (SnakeGameManagerSV manager in snakeGameManagerSVCollection.Values)
            {
                manager.RequestAuxGameLoopThreadToEnd();
            }

            // Close all sockets

            using (IEnumerator<string> enumerator = tempIdClientSocketPairs.Keys.ToList().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string clientID = enumerator.Current;

                    AttemptToDisconnectClientFromServer(tempIdClientSocketPairs[clientID], clientID);
                }
            }

            using (IEnumerator<string> enumerator = idClientSocketPairs.Keys.ToList().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string clientID = enumerator.Current;

                    AttemptToDisconnectClientFromServer(idClientSocketPairs[clientID], clientID);
                }
            }

            tempIdClientSocketPairs.Clear();
            idClientSocketPairs.Clear();

            serverSocket.Close();
            serverSocket.Dispose();
            serverSocket = null;
            
            serverLogTextBox.Clear();
        }
    }
}
