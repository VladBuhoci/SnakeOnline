using System.Drawing;

namespace SnakeOnlineCore
{
    public class FoodObject : SnakeGameArenaObject
    {
        /// <summary>
        ///     Value indicates how many parts the snake will grow if it eats this food item.
        ///     ... or how many it loses if it eats the bad one (not yet implemented).
        /// </summary>
        public readonly int bodyPartsAmount;

        public static string EFFECT_NOTHING = "nothing.";
        public static string EFFECT_GROW_PARTS = "growing body parts.";
        public static string EFFECT_LOSE_PARTS = "losing body parts.";

        public FoodObject(int posX, int posY, Color color, int bodyPartsAmount)
            : base(posX, posY, color)
        {
            this.bodyPartsAmount = bodyPartsAmount;
        }
    }
}