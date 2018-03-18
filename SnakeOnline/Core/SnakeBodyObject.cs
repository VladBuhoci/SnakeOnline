using System.Drawing;

namespace SnakeOnline.Core
{
    class SnakeBodyObject : SnakeGameArenaObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public bool isHead;

        public Color color { get; set; }

        public SnakeBodyObject(int posX, int posY, bool isHead, Color color)
        {
            this.posX = posX;
            this.posY = posY;
            this.isHead = isHead;
            this.color = color;
        }
    }
}
