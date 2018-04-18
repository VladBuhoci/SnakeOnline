using System;

namespace SnakeOnlineCore
{
    [Serializable]
    class CommunicationPacketWrapper
    {
        public int uniquePlayerID { get; set; }
        public int uniqueGameManagerID { get; set; }
        public CommunicationProtocol protocolCommand { get; set; }
        public Object data { get; set; }

        public CommunicationPacketWrapper(int uniquePlayerID, int uniqueGameManagerID, CommunicationProtocol protocolCommand, Object data)
        {
            this.uniquePlayerID = uniquePlayerID;
            this.uniqueGameManagerID = uniqueGameManagerID;
            this.protocolCommand = protocolCommand;
            this.data = data;
        }
    }
}