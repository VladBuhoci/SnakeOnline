using System.Drawing;

namespace SnakeOnline.Core
{
    class SnakeBodyObject : SnakeGameArenaObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public bool isHead;

        public Color color { get; set; }

        public Snake snake;

        public SnakeBodyObject(int posX, int posY, bool isHead, Color color, Snake snake)
        {
            this.posX = posX;
            this.posY = posY;
            this.isHead = isHead;
            this.color = color;
            this.snake = snake;
        }
    }
}
