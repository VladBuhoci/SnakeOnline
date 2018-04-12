using System.Drawing;

namespace SnakeOnlineCore
{
    public class SnakeBodyObject : SnakeGameArenaObject
    {
        public bool isHead;

        public Snake snake;

        public SnakeBodyObject(int posX, int posY, bool isHead, Color color, Snake snake)
            : base(posX, posY, color)
        {
            this.isHead = isHead;
            this.snake = snake;
        }
    }
}