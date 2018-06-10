using System;
using System.Drawing;

namespace SnakeOnlineCore
{
    [Serializable]
    public class SnakeBodyObject : SnakeGameArenaObject
    {
        public bool isHead;

        //public Snake snake;
        public string snakeID;

        public SnakeBodyObject(int posX, int posY, bool isHead, Color color, /*Snake snake*/string snakeID)
            : base(posX, posY, color)
        {
            this.isHead = isHead;
            //this.snake = snake;
            this.snakeID = snakeID;
        }
    }
}