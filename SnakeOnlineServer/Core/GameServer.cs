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

        private static byte[] dataBuffer = new byte[1024];  // TODO: needed as a class field?

        public GameServer(TextBox _serverLogTextBox)
        {
            serverLogTextBox = _serverLogTextBox;

            clientSocketList = new List<Socket>();
            serverSocket     = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void SetUpServer()
        {
            LogMessage("Attempting to start server...");

            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1702));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(ServerAcceptConnectionCallback), null);
        }

        private static void ServerAcceptConnectionCallback(IAsyncResult AR)
        {
            try
            {
                Socket newClientSocket = serverSocket.EndAccept(AR);
                newClientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), newClientSocket);

                clientSocketList.Add(newClientSocket);

                LogMessage("New client is now connected to the server.");

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
                byte[] dataBuf = new byte[receivedDataSize];

                Array.Copy(dataBuffer, dataBuf, receivedDataSize);

                // Log received data
                LogMessage("Data received: " + Encoding.ASCII.GetString(dataBuf));

                // TODO: handle the received data.
                //SendData();
                //byte[] dataToSendBackToClient = ...
                //clientSocket.BeginSend(dataToSendBackToClient, 0, dataToSendBackToClient.Length, SocketFlags.None, new AsyncCallback(ServerSendToClientCallback), clientSocket);

                // Resume receiving data from this client socket.
                clientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ServerBeginReceiveDataFromClient), clientSocket);
            }
            catch
            {
                LogMessage("~ Client disconnected.");

                clientSocketList.Remove(clientSocket);

                clientSocket.Close();
                clientSocket.Dispose();
                clientSocket = null;
            }
        }

        private static void SendData()
        {

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
                serverLogTextBox.AppendText(text + "\n");
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
