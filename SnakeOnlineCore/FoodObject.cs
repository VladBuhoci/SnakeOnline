using System;
using System.Drawing;

namespace SnakeOnlineCore
{
    [Serializable]
    public class FoodObject : SnakeGameArenaObject
    {
        /// <summary>
        ///     Value indicates how many parts the snake will grow if it eats this food item.
        /// </summary>
        public readonly int bodyPartsAmount;
        
        public FoodObject(int posX, int posY, Color color, int bodyPartsAmount)
            : base(posX, posY, color)
        {
            this.bodyPartsAmount = bodyPartsAmount;
        }
    }
}