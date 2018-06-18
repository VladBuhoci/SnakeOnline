using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnlineCore
{
    public class AsyncStateContainer
    {
        public Socket socket { get; set; }
        public byte[] dataBuffer;

        public AsyncStateContainer(Socket _socket)
        {
            socket = _socket;
            dataBuffer = new byte[8192];
        }
    }
}
