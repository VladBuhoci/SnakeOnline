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

        public SnakeGameManagerSV(int arenaWidth, int arenaHeight, Dictionary<int, Snake> idSnakePairs)
        {
            bApplicationAttemptsClosing = false;
            gameArenaWidth = arenaWidth;
            gameArenaHeight = arenaHeight;
            snakes = idSnakePairs;
            gameArenaObjects = new SnakeGameArenaObject[gameArenaWidth, gameArenaHeight];
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }
    }
}
