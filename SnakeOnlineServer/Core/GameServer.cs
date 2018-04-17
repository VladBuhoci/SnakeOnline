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
        private static TextBox serverLogTextBox;

        private static Socket serverSocket;
        private static List<Socket> clientSocketList;

        private static byte[] rawDataBuffer = new byte[1024];  // TODO: needed as a class field?
        private static int uniqueIDCounter;

        public GameServer(TextBox _serverLogTextBox)
        {
            serverLogTextBox = _serverLogTextBox;

            serverSocket     = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocketList = new List<Socket>();
            uniqueIDCounter  = 0;
        }

        public void SetUpServer()
        {
            LogMessage("Attempting to start server...");

            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1702));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(ServerAcceptConnectionCallback), null);

            LogMessage("Server has started.");
        }

        private static void ServerAcceptConnectionCallback(IAsyncResult AR)
        {
            try
            {
                Socket newClientSocket = serverSocket.EndAccept(AR);

                // Begin receiving data from this client.
                newClientSocket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), newClientSocket);

                // Send the unique ID to this client.
                byte[] idData = CommunicationProtocolUtils.MakeNetworkCommand(-1, CommunicationProtocol.SEND_PLAYER_ID, uniqueIDCounter);
                newClientSocket.Send(idData);

                clientSocketList.Add(newClientSocket);

                LogMessage(String.Format("New client is now connected to the server. (id: {0})", uniqueIDCounter));
                uniqueIDCounter += 1;

                // Resume accepting connections.
                serverSocket.BeginAccept(new AsyncCallback(ServerAcceptConnectionCallback), null);
            }
            catch
            {
                //MessageBox.Show(null, "There was a problem in trying to accept a new client connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ServerBeginReceiveDataFromClient(IAsyncResult AR)
        {
            Socket clientSocket = (Socket) AR.AsyncState;

            try
            {
                int receivedDataSize = clientSocket.EndReceive(AR);
                byte[] actualDataBuffer = new byte[receivedDataSize];

                Array.Copy(rawDataBuffer, actualDataBuffer, receivedDataSize);

                // Handle the received data.
                HandleReceivedData(actualDataBuffer);
                
                // TODO #1: after handling the data, send things back to the client(s).
                // TODO #2: is this really where (and how) to do it?
                //SendDataToClient();

                // Resume receiving data from this client socket.
                clientSocket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), clientSocket);
            }
            catch (Exception e)
            {
                LogMessage("~ Client disconnected.");
                LogMessage(e.StackTrace);

                clientSocketList.Remove(clientSocket);

                clientSocket.Close();
                clientSocket.Dispose();
                clientSocket = null;
            }
        }

        private static void HandleReceivedData(byte[] dataBuffer)
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

                default:
                    LogMessage("Unknown command received from client.");
                    break;
            }
        }

        private static void SendDataToClient()
        {
            //byte[] dataToSendBackToClient = ...
            //clientSocket.BeginSend(dataToSendBackToClient, 0, dataToSendBackToClient.Length, SocketFlags.None, new AsyncCallback(ServerSendToClientCallback), clientSocket);
        }

        private static void ServerSendToClientCallback(IAsyncResult AR)
        {
            Socket clientSocket = (Socket) AR.AsyncState;

            clientSocket.EndSend(AR);
        }

        private static void LogMessage(String text)
        {
            if (serverLogTextBox != null)
            {
                serverLogTextBox.BeginInvoke(new MethodInvoker(delegate { serverLogTextBox.AppendText(text + "\n"); }));
            }
        }

        /// <summary>
        ///     Clean up method.
        ///     W.I.P.
        /// </summary>
        public void CleanUp()
        {
            serverLogTextBox.Clear();

            // close all sockets
            foreach (Socket clientSocket in clientSocketList)
            {
                clientSocket.Close();
                clientSocket.Dispose();
            }

            clientSocketList.Clear();

            serverSocket.Close();
            serverSocket.Dispose();
            serverSocket = null;
        }
    }
}
