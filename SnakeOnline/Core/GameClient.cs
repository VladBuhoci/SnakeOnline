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

namespace SnakeOnline.Core
{
    class GameClient
    {
        private Socket socket;

        private int serverPortNumber;
        private int uniquePlayerID;
        private int currentUniqueGameManagerID;
        private byte[] rawDataBuffer = new byte[1024];

        public ClientLobbyWindow clientLobbyWindow { get; set; }
        public SnakeGameManagerCL snakeGameManagerCL { get; set; }

        public GameClient()
        {
            SetUpClientSocket();
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
                catch (SocketException)
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

                // Handle the received data.
                HandleReceivedData(actualDataBuffer);

                // TODO: any code to send data back to server should be done here.
                
                // Resume receiving data from the server.
                socket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(BeginReceiveDataFromServer), socket);
            }
            catch
            {
                socket.Close();
                socket.Dispose();
                socket = null;
            }
        }

        private void HandleReceivedData(byte[] dataBuffer)
        {
            //if (CommunicationProtocolUtils.GetPlayerIDFromCommand(dataBuffer) == -1)
            //{
                CommunicationProtocol command = CommunicationProtocolUtils.GetProtocolValueFromCommand(dataBuffer);

                switch (command)
                {
                    case CommunicationProtocol.SEND_PLAYER_ID:
                        // Save the ID provided by the server.
                        // This only happens when we connect to the server.
                        uniquePlayerID = (int) CommunicationProtocolUtils.GetDataFromCommand(dataBuffer);
                        break;

                    case CommunicationProtocol.SEND_GAME_MANAGER_ID:
                        currentUniqueGameManagerID = CommunicationProtocolUtils.GetGameManagerIDFromCommand(dataBuffer);

                        Form gameWindow = new ClientGameWindow(this, clientLobbyWindow, currentUniqueGameManagerID);
                        clientLobbyWindow.Visible = false;
                        gameWindow.ShowDialog(clientLobbyWindow);

                        break;

                    case CommunicationProtocol.SEND_ARENA_MATRIX:
                        // TODO
                        break;
                }
            //}
        }

        // TODO: useful?
        public void SendDataToServer(byte[] data)
        {
            socket.Send(data);
        }

        public void SendSnakeSpawnRequestToServer()
        {
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, snakeGameManagerCL.GetUniqueGameManagerID(), CommunicationProtocol.SPAWN_SNAKE, "NODATA"));
        }

        public void SendCreateGameRequestToServer()
        {
            // TODO: add every client id in the data field?
            socket.Send(CommunicationProtocolUtils.MakeNetworkCommand(uniquePlayerID, -1, CommunicationProtocol.CREATE_GAME, "NODATA YET | probably many client IDs"));
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
