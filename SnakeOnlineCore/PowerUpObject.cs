using System;
using System.Drawing;

namespace SnakeOnlineCore
{
    [Serializable]
    public class PowerUpObject : SnakeGameArenaObject
    {
        // Measured in seconds.
        public readonly int effectDuration;

        public PowerUpObject(int posX, int posY, Color color, int effectDuration = 3)
            : base(posX, posY, color)
        {
            this.effectDuration = effectDuration;
        }
    }
}