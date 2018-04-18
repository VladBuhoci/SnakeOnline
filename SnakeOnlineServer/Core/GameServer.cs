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
        private Dictionary<int, Socket> idClientSocketPairs;
        private Dictionary<int, SnakeGameManagerSV> snakeGameManagerSVCollection;

        private byte[] rawDataBuffer = new byte[1024];  // TODO: needed as a class field?
        private int uniquePlayerIDCounter;
        private int uniqueGameManagerIDCounter;

        public GameServer(TextBox _serverLogTextBox)
        {
            serverLogTextBox             = _serverLogTextBox;
            snakeGameManagerSVCollection = new Dictionary<int, SnakeGameManagerSV>();

            serverSocket                 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            idClientSocketPairs          = new Dictionary<int, Socket>();
            uniquePlayerIDCounter        = 0;
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

                // Begin receiving data from this client.
                newClientSocket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), newClientSocket);

                // Send the unique ID to this client.
                byte[] idData = CommunicationProtocolUtils.MakeNetworkCommand(-1, 0, CommunicationProtocol.SEND_PLAYER_ID, uniquePlayerIDCounter);
                newClientSocket.Send(idData);

                idClientSocketPairs.Add(uniquePlayerIDCounter, newClientSocket);

                LogMessage(String.Format("New client is now connected to the server. (id: {0})", uniquePlayerIDCounter));
                uniquePlayerIDCounter += 1;

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

                int key = -1;
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

                if (key != -1)
                {
                    idClientSocketPairs.Remove(key);
                }

                clientSocket.Close();
                clientSocket.Dispose();
                clientSocket = null;
            }
        }

        private void HandleReceivedData(byte[] dataBuffer, Socket clientSocket)
        {
            CommunicationProtocol command = CommunicationProtocolUtils.GetProtocolValueFromCommand(dataBuffer);

            switch (command)
            {
                case CommunicationProtocol.SPAWN_SNAKE:
                    LogMessage("Player requested a snake to be created.");

                    // Create a snake and a unique ID and return the ID to the player.
                    // ... but not here. The right place is the game manager.
                    // ... also, it might be better to just generate an ID for each player when they connect for the first time.

                    break;

                case CommunicationProtocol.CREATE_GAME:
                    LogMessage("New game request.");

                    int playerID = CommunicationProtocolUtils.GetPlayerIDFromCommand(dataBuffer);

                    // Create the game manager here and do thingies for it.
                    // ... also add it to the dictionary.
                    // ... NOTE: the command wrapper should contain every client ID that will participate in this match in the data field.

                    // foreach (clientID in list of IDs)
                    // Think of the data that could be sent here...
                    byte[] newManagerResultCommand = CommunicationProtocolUtils.MakeNetworkCommand(-1, uniqueGameManagerIDCounter, CommunicationProtocol.SEND_GAME_MANAGER_ID, playerID);

                    // Send the result back.
                    idClientSocketPairs[playerID].Send(newManagerResultCommand);

                    //snakeGameManagerSVCollection.Add(uniqueGameManagerIDCounter, newManager);

                    uniqueGameManagerIDCounter += 1;

                    break;

                default:
                    LogMessage("Unknown command received from client.");
                    break;
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
            foreach (Socket clientSocket in idClientSocketPairs.Values)
            {
                clientSocket.Close();
                clientSocket.Dispose();
            }

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
