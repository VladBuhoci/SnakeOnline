using System;

namespace SnakeOnlineCore
{
    [Serializable]
    class CommunicationPacketWrapper
    {
        public string uniquePlayerID { get; set; }
        public int uniqueGameManagerID { get; set; }
        public CommunicationProtocol protocolCommand { get; set; }
        public Object data { get; set; }

        public CommunicationPacketWrapper(string uniquePlayerID, int uniqueGameManagerID, CommunicationProtocol protocolCommand, Object data)
        {
            this.uniquePlayerID = uniquePlayerID;
            this.uniqueGameManagerID = uniqueGameManagerID;
            this.protocolCommand = protocolCommand;
            this.data = data;
        }
    }
}