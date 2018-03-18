using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline.Core
{
    class SnakeController
    {
        // TODO: temporary..
        Snake controlledSnake { get; set; }

        private SnakeOrientation currentOrientation;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public SnakeController(int snakePosX, int snakePosY, Color snakeColor)
        {
            // TODO: temporary.. should create the snake on the server in the future.
            controlledSnake = new Snake(snakePosX, snakePosY, snakeColor);
        }

        public void MoveUp()
        {
            if (currentOrientation != SnakeOrientation.Up)
            {
                currentOrientation = SnakeOrientation.Up;

                // TODO: temporary..
                controlledSnake.MoveUp();
            }
        }

        public void MoveRight()
        {
            if (currentOrientation != SnakeOrientation.Right)
            {
                currentOrientation = SnakeOrientation.Right;

                // TODO: temporary..
                controlledSnake.MoveRight();
            }
        }

        public void MoveDown()
        {
            if (currentOrientation != SnakeOrientation.Down)
            {
                currentOrientation = SnakeOrientation.Down;

                // TODO: temporary..
                controlledSnake.MoveDown();
            }
        }

        public void MoveLeft()
        {
            if (currentOrientation != SnakeOrientation.Left)
            {
                currentOrientation = SnakeOrientation.Left;

                // TODO: temporary..
                controlledSnake.MoveLeft();
            }
        }
    }
}
