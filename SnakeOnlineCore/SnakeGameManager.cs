using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnlineCore
{
    public sealed class SnakeGameManager
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

            // Start the game loop thread.

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
                
                Thread.Sleep(100);
            }

            Thread.CurrentThread.Abort();
        }

        /// <summary>
        ///     Will randomly generate a food object in an empty spot of the arena.
        /// </summary>
        public void SpawnFood(Color color)
        {
            int randPosX, randPosY, randAmount;
            Random random = new Random(DateTime.Now.Millisecond);

            randPosX = random.Next(0, gameArenaWidth);
            randPosY = random.Next(0, gameArenaHeight);
            randAmount = random.Next(1, 4);

            FoodObject food = new FoodObject(randPosX, randPosY, color, randAmount);

            AddFood(food);
        }

        private void AddFood(FoodObject food)
        {
            gameArenaObjects[food.posX, food.posY] = food;
        }

        public void FoodWasEaten(FoodObject food)
        {
            gameArenaObjects[food.posX, food.posY] = null;

            SpawnFood(food.color);
        }

        public void AddSnake(Snake snake)
        {
            snakes.Add(snake);

            foreach (SnakeBodyObject snakePart in snake.GetSnakeBodyParts())
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
