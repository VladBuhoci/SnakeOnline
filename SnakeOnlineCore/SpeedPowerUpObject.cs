using System;
using System.Drawing;

namespace SnakeOnlineCore
{
    [Serializable]
    public class SpeedPowerUpObject : PowerUpObject
    {
        public readonly int speedAmplifier;

        public SpeedPowerUpObject(int posX, int posY, Color color, int effectDuration, int speedAmplifier = 2)
            : base(posX, posY, color, effectDuration)
        {
            this.speedAmplifier = speedAmplifier;
        }
    }
}