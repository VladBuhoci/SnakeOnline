using System;

namespace SnakeOnlineCore
{
    [Serializable]
    public struct SnakeGameDescriptor
    {
        // Room properties.
        public int gameManagerID { get; set; }
        public string roomName { get; set; }
        public string roomPassword { get; set; }
        public string roomLeaderID { get; set; }
        public int currentPlayerCount { get; set; }
        public int currentSpectatorCount { get; set; }
        public int totalGameRoomTime { get; set; }
        public GameRoomState roomState { get; set; }

        // Game properties. (or rules)
        public int arenaWidth { get; set; }
        public int arenaHeight { get; set; }
        public int maxSnakesAllowed { get; set; }
        public int matchDuration { get; set; }      // Measured in minutes.
    }
}