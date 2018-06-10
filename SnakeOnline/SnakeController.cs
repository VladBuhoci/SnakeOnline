using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline
{
    class SnakeController
    {
        private GameClient socket;

        private SnakeOrientation currentOrientation;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public SnakeController(GameClient client, SnakeOrientation snakeOrientation)
        {
            socket = client;
            currentOrientation = snakeOrientation;
        }

        public void ChangeDirectionUp()
        {
            if (currentOrientation != SnakeOrientation.Up && currentOrientation != SnakeOrientation.Down)
            {
                currentOrientation = SnakeOrientation.Up;

                // TODO: temporary..
                //controlledSnake.ChangeDirection(SnakeOrientation.Up);
            }
        }

        public void ChangeDirectionRight()
        {
            if (currentOrientation != SnakeOrientation.Right && currentOrientation != SnakeOrientation.Left)
            {
                currentOrientation = SnakeOrientation.Right;

                // TODO: temporary..
                //controlledSnake.ChangeDirection(SnakeOrientation.Right);
            }
        }

        public void ChangeDirectionDown()
        {
            if (currentOrientation != SnakeOrientation.Down && currentOrientation != SnakeOrientation.Up)
            {
                currentOrientation = SnakeOrientation.Down;

                // TODO: temporary..
                //controlledSnake.ChangeDirection(SnakeOrientation.Down);
            }
        }

        public void ChangeDirectionLeft()
        {
            if (currentOrientation != SnakeOrientation.Left && currentOrientation != SnakeOrientation.Right)
            {
                currentOrientation = SnakeOrientation.Left;

                // TODO: temporary..
                //controlledSnake.ChangeDirection(SnakeOrientation.Left);
            }
        }
    }
}
