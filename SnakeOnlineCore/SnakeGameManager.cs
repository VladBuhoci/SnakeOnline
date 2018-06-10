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
    public abstract class SnakeGameManager
    {
        protected bool bApplicationAttemptsClosing;

        protected const int TIME_BEFORE_GAME_STARTS = 3;  // in seconds.

        protected List<Snake> snakes = new List<Snake>();

        public int gameArenaWidth { get; set; }
        public int gameArenaHeight { get; set; }
        public SnakeGameArenaObject[, ] gameArenaObjects { get; set; }
        
        public SnakeGameManager(int arenaWidth, int arenaHeight)
        {
            bApplicationAttemptsClosing = false;

            gameArenaWidth = arenaWidth;
            gameArenaHeight = arenaHeight;
            gameArenaObjects = new SnakeGameArenaObject[gameArenaWidth, gameArenaHeight];
        }

        public void StartGameLoop()
        {
            // Start the game loop thread.

            Thread auxGameLoopThread = new Thread(() => GameLoop(this, snakes));
            auxGameLoopThread.Start();
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }

        protected abstract void GameLoop(SnakeGameManager gameManager, List<Snake> snakeList);

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

        public abstract void KillSnake(string snakeID);
    }
}