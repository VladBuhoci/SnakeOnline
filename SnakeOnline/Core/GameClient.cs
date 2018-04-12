using System;
using System.Collections.Generic;
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
        private static Socket clientSocket;

        private int serverPortNumber;

        public GameClient()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPortNumber = 1702;

            LoopConnect();
        }

        private void LoopConnect()
        {
            int attempts = 0;

            while (! clientSocket.Connected)
            {
                // TODO: find some usage for this.
                attempts += 1;

                try
                {
                    clientSocket.Connect(IPAddress.Loopback, serverPortNumber);
                }
                catch (SocketException)
                {
                    //MessageBox.Show("Cannot connect to server :(");
                }
            }
        }

        public void SendTestData(byte[] data)
        {
            clientSocket.Send(data);
        }
    }
}
