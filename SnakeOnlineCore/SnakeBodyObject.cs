using System;
using System.Drawing;

namespace SnakeOnlineCore
{
    [Serializable]
    public class SnakeBodyObject : SnakeGameArenaObject
    {
        public bool isHead;

        public string snakeID;

        public SnakeBodyObject(int posX, int posY, bool isHead, Color color, string snakeID)
            : base(posX, posY, color)
        {
            this.isHead = isHead;
            this.snakeID = snakeID;
        }
    }
}