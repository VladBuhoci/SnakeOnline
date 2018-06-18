using System;

namespace SnakeOnlineCore
{
    [Serializable]
    struct SocpMessageContainer
    {
        public string uniquePlayerID { get; set; }
        public int uniqueGameManagerID { get; set; }
        public Socp protocolCommand { get; set; }
        public Object data { get; set; }

        public SocpMessageContainer(string uniquePlayerID, int uniqueGameManagerID, Socp protocolCommand, Object data)
        {
            this.uniquePlayerID = uniquePlayerID;
            this.uniqueGameManagerID = uniqueGameManagerID;
            this.protocolCommand = protocolCommand;
            this.data = data;
        }
    }
}