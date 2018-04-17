using System;

namespace SnakeOnlineCore
{
    [Serializable]
    class CommunicationPacketWrapper
    {
        public int uniqueID { get; set; }
        public CommunicationProtocol protocolCommand { get; set; }
        public Object data { get; set; }

        public CommunicationPacketWrapper(int uniqueID, CommunicationProtocol protocolCommand, Object data)
        {
            this.uniqueID = uniqueID;
            this.protocolCommand = protocolCommand;
            this.data = data;
        }
    }
}