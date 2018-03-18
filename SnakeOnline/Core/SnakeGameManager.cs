using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Core
{
    abstract class SnakeGameManager
    {
        public static SnakeGameArenaObject[, ] gameArenaObjects = new SnakeGameArenaObject[50, 50];
        
        /// <summary>
        ///     Will randomly generate a food object in an empty spot of the arena.
        /// </summary>
        public static void SpawnFood()
        {

        }

        public static void AddSnake(Queue<SnakeBodyObject> snakeParts)
        {
            foreach (SnakeBodyObject snakePart in snakeParts)
            {
                gameArenaObjects[snakePart.posX, snakePart.posY] = snakePart;
            }
        }
    }
}
