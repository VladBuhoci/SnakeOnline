using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnlineServer
{
    class SnakeGameManagerSV
    {
        private bool bApplicationAttemptsClosing;

        private int gameArenaWidth;
        private int gameArenaHeight;
        private SnakeGameArenaObject[,] gameArenaObjects;
        private Dictionary<string, Snake> playerWithSnakeCollection;
        private List<string> spectatorList;
        private string eatenFoodEffect;                    // NOTE: there are constants defined in the FoodObject class for this.
        private int matchDuration;

        private GameServer gameServer;

        private string roomLeaderID;

        public SnakeGameManagerSV(int arenaWidth, int arenaHeight, string foodEffect, int duration, GameServer server, string leaderID)
        {
            bApplicationAttemptsClosing = false;

            gameArenaWidth = arenaWidth;
            gameArenaHeight = arenaHeight;
            gameArenaObjects = new SnakeGameArenaObject[gameArenaWidth, gameArenaHeight];
            playerWithSnakeCollection = new Dictionary<string, Snake>();
            spectatorList = new List<string>();
            eatenFoodEffect = foodEffect;
            matchDuration = duration;

            gameServer = server;

            roomLeaderID = leaderID;

            // Add the leader to the Players With Snakes collection automatically.
            playerWithSnakeCollection.Add(roomLeaderID, null);
        }

        public void RemovePlayerFromGame(string playerID)
        {
            if (playerWithSnakeCollection.Keys.Contains(playerID))
            {
                playerWithSnakeCollection.Remove(playerID);

                // TODO
            }
            else if (spectatorList.Contains(playerID))
            {
                spectatorList.Remove(playerID);

                // TODO
            }

            // TODO: more actions to be handled here, including a check whether this
            //          was the room leader, in which case we have to find a new leader.
            //          If there are no players left, perhaps ask the server to kill the room.
        }

        /// <summary>
        ///     Returns true if there are no players left in this game room.
        /// </summary>
        public bool IsGameEmpty()
        {
            return playerWithSnakeCollection.Count == 0 && spectatorList.Count == 0;
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }
    }
}