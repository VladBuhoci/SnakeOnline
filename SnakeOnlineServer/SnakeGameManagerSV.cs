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
        private Dictionary<int, Snake> snakes;
        private string eatenFoodEffect;                    // NOTE: there are constants defined in the FoodObject class for this.
        private int matchDuration;

        private GameServer gameServer;

        public SnakeGameManagerSV(int arenaWidth, int arenaHeight, string foodEffect, int duration, GameServer server)
        {
            bApplicationAttemptsClosing = false;

            gameArenaWidth = arenaWidth;
            gameArenaHeight = arenaHeight;
            gameArenaObjects = new SnakeGameArenaObject[gameArenaWidth, gameArenaHeight];
            snakes = new Dictionary<int, Snake>();
            eatenFoodEffect = foodEffect;
            matchDuration = duration;

            gameServer = server;
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }
    }
}