using System.Drawing;

namespace SnakeOnlineCore
{
    public class SnakeGameArenaObject
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public Color color { get; set; }

        public SnakeGameArenaObject(int posX, int posY, Color color)
        {
            this.posX = posX;
            this.posY = posY;
            this.color = color;
        }
    }
}