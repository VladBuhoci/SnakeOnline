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
        public SnakeController(int snakePosX, int snakePosY, Color snakeColor, SnakeOrientation snakeOrientation = SnakeOrientation.Right, int bodyLength = 4)
        {
            // TODO: temporary.. should create the snake on the server in the future.
            controlledSnake = new Snake(snakePosX, snakePosY, snakeColor, snakeOrientation, bodyLength);

            currentOrientation = snakeOrientation;
        }

        public Snake GetControlledSnake()
        {
            return controlledSnake;
        }

        public void ChangeDirectionUp()
        {
            if (currentOrientation != SnakeOrientation.Up && currentOrientation != SnakeOrientation.Down)
            {
                currentOrientation = SnakeOrientation.Up;

                // TODO: temporary..
                controlledSnake.ChangeDirection(SnakeOrientation.Up);
            }
        }

        public void ChangeDirectionRight()
        {
            if (currentOrientation != SnakeOrientation.Right && currentOrientation != SnakeOrientation.Left)
            {
                currentOrientation = SnakeOrientation.Right;

                // TODO: temporary..
                controlledSnake.ChangeDirection(SnakeOrientation.Right);
            }
        }

        public void ChangeDirectionDown()
        {
            if (currentOrientation != SnakeOrientation.Down && currentOrientation != SnakeOrientation.Up)
            {
                currentOrientation = SnakeOrientation.Down;

                // TODO: temporary..
                controlledSnake.ChangeDirection(SnakeOrientation.Down);
            }
        }

        public void ChangeDirectionLeft()
        {
            if (currentOrientation != SnakeOrientation.Left && currentOrientation != SnakeOrientation.Right)
            {
                currentOrientation = SnakeOrientation.Left;

                // TODO: temporary..
                controlledSnake.ChangeDirection(SnakeOrientation.Left);
            }
        }
    }
}
