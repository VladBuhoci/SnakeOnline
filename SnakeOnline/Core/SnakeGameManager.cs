using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline.Core
{
    sealed class SnakeGameManager
    {
        private static SnakeGameManager classInstance = new SnakeGameManager();
        private static bool bApplicationAttemptsClosing;

        private List<Snake> snakes = new List<Snake>();
        private Panel gameArenaPanel;

        public int gameArenaWidth = 50;
        public int gameArenaHeight = 50;
        public SnakeGameArenaObject[, ] gameArenaObjects;
        
        private SnakeGameManager()
        {
            gameArenaObjects = new SnakeGameArenaObject[gameArenaWidth, gameArenaHeight];
        }

        public static SnakeGameManager GetInstance()
        {
            return classInstance;
        }

        public void SetGameArenaPanel(Panel gamePanel)
        {
            gameArenaPanel = gamePanel;

            Thread auxGameLoopThread = new Thread(() => GameLoop(snakes, gameArenaPanel));
            auxGameLoopThread.Start();
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }

        private static void GameLoop(List<Snake> snakeList, Panel gamePanel)
        {
            while (true)
            {
                if (bApplicationAttemptsClosing)
                {
                    break;
                }

                foreach (Snake snake in snakeList)
                {
                    snake.MoveSnake();
                }

                // Work-around: some things shouldn't "normally" be accessed from any threads other than their original one.
                if (gamePanel != null && gamePanel.IsHandleCreated)
                {
                    gamePanel.BeginInvoke(new MethodInvoker(delegate { gamePanel.Refresh(); }));
                }

                Thread.Sleep(100);
            }

            Thread.CurrentThread.Abort();
        }

        /// <summary>
        ///     Will randomly generate a food object in an empty spot of the arena.
        /// </summary>
        public void SpawnFood()
        {
            
        }

        public void AddSnake(Snake snake, Queue<SnakeBodyObject> snakeParts)
        {
            snakes.Add(snake);

            foreach (SnakeBodyObject snakePart in snakeParts)
            {
                gameArenaObjects[snakePart.posX, snakePart.posY] = snakePart;
            }
        }

        public void KillSnake(Snake victim)
        {
            victim.bIsAlive = false;

            // other stuff
        }
    }
}
