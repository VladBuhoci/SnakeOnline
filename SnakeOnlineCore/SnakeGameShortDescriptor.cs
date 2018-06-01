using System;

namespace SnakeOnlineCore
{
    [Serializable]
    public struct SnakeGameShortDescriptor
    {
        public int gameManagerID { get; set; }
        public string roomName { get; set; }
        public bool hasPassword { get; set; }
        public int currentPlayerCount { get; set; }
        public int currentSpectatorCount { get; set; }
        public GameRoomState roomState { get; set; }
    }
}