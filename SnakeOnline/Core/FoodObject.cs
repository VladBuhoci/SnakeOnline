using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Core
{
    class FoodObject : SnakeGameArenaObject
    {
        /// <summary>
        ///     Value indicates how many parts the snake will grow if it eats this food item.
        ///     ... or how many it loses if it eats the bad one (not yet implemented).
        /// </summary>
        public readonly int bodyPartsAmount;

        public FoodObject(int posX, int posY, Color color, int bodyPartsAmount)
            : base(posX, posY, color)
        {
            this.bodyPartsAmount = bodyPartsAmount;
        }
    }
}
