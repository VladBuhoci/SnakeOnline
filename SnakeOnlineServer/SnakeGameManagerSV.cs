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

        private int id;

        private int gameArenaWidth;
        private int gameArenaHeight;
        private SnakeGameArenaObject[,] gameArenaObjects;
        private Dictionary<string, Snake> playerWithSnakeCollection;
        private List<string> spectatorList;
        private string eatenFoodEffect;                    // NOTE: there are constants defined in the FoodObject class for this.
        private int matchDuration;

        private GameServer gameServer;

        private string roomLeaderID;

        public SnakeGameManagerSV(int managerID, int arenaWidth, int arenaHeight, string foodEffect, int duration, GameServer server, string leaderID)
        {
            bApplicationAttemptsClosing = false;

            id = managerID;

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

        public void SwitchSidesForClient(string clientID)
        {
            if (playerWithSnakeCollection.Keys.Contains(clientID))
            {
                playerWithSnakeCollection.Remove(clientID);

                spectatorList.Add(clientID);
            }
            else if (spectatorList.Contains(clientID))
            {
                spectatorList.Remove(clientID);

                playerWithSnakeCollection.Add(clientID, null);
            }

            gameServer.BroadcastPlayerListForRoom(id);
            gameServer.BroadcastSpectatorListForRoom(id);
        }

        public void RemoveClientFromGame(string clientID)
        {
            if (playerWithSnakeCollection.Keys.Contains(clientID))
            {
                playerWithSnakeCollection.Remove(clientID);

                gameServer.BroadcastPlayerListForRoom(id);
            }
            else if (spectatorList.Contains(clientID))
            {
                spectatorList.Remove(clientID);

                gameServer.BroadcastSpectatorListForRoom(id);
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

        public string[] Players
        {
            get
            {
                return playerWithSnakeCollection.Keys.ToArray();
            }
        }

        public string[] Spectators
        {
            get
            {
                return spectatorList.ToArray();
            }
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }
    }
}