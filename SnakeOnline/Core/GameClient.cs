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
        private static Socket socket;

        private int serverPortNumber;
        private int uniqueID;
        private byte[] rawDataBuffer = new byte[1024];

        public GameClient()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPortNumber = 1702;

            LoopConnect();
            WaitReceiveAndHandleDataFromServer();
        }

        private void LoopConnect()
        {
            int attempts = 0;

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

                // TODO: handle the received data.
                HandleReceivedData(actualDataBuffer);

                // TODO: any code to send data back to server should done here.
                
                // Resume receiving data from the server.
                socket.BeginReceive(rawDataBuffer, 0, rawDataBuffer.Length, SocketFlags.None, new AsyncCallback(BeginReceiveDataFromServer), socket);
            }
            catch
            {
                socket.Close();
                socket.Dispose();
                socket = null;

                MessageBox.Show(null, "Disconnected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
        }

        private void HandleReceivedData(byte[] dataBuffer)
        {
            if (CommunicationProtocolUtils.GetIDFromCommand(dataBuffer) == -1)
            {
                CommunicationProtocol command = CommunicationProtocolUtils.GetProtocolFromCommand(dataBuffer);

                switch (command)
                {
                    case CommunicationProtocol.SEND_PLAYER_ID:
                        uniqueID = CommunicationProtocolUtils.GetIDFromCommand(dataBuffer);
                        break;

                    case CommunicationProtocol.SEND_ARENA_MATRIX:
                        // TODO
                        break;
                }
            }
        }

        // TODO: useful?
        public void SendDataToServer(byte[] data)
        {
            socket.Send(data);
        }

        public void SendSnakeSpawnRequestToServer()
        {
            socket.Send(CommunicationProtocolUtils.MakeCommand(uniqueID, CommunicationProtocol.SPAWN_SNAKE, "NODATA"));
        }
    }
}
