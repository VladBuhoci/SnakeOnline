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
        private int gameRoomID;
        private SnakeOrientation currentOrientation;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public SnakeController(GameClient client, int roomID, SnakeOrientation snakeOrientation)
        {
            socket = client;
            gameRoomID = roomID;
            currentOrientation = snakeOrientation;
        }

        public void ChangeDirectionUp()
        {
            if (currentOrientation != SnakeOrientation.Up && currentOrientation != SnakeOrientation.Down)
            {
                currentOrientation = SnakeOrientation.Up;

                socket.SendChangeSnakeOrientationRequest(gameRoomID, SnakeOrientation.Up);
            }
        }

        public void ChangeDirectionRight()
        {
            if (currentOrientation != SnakeOrientation.Right && currentOrientation != SnakeOrientation.Left)
            {
                currentOrientation = SnakeOrientation.Right;

                socket.SendChangeSnakeOrientationRequest(gameRoomID, SnakeOrientation.Right);
            }
        }

        public void ChangeDirectionDown()
        {
            if (currentOrientation != SnakeOrientation.Down && currentOrientation != SnakeOrientation.Up)
            {
                currentOrientation = SnakeOrientation.Down;

                socket.SendChangeSnakeOrientationRequest(gameRoomID, SnakeOrientation.Down);
            }
        }

        public void ChangeDirectionLeft()
        {
            if (currentOrientation != SnakeOrientation.Left && currentOrientation != SnakeOrientation.Right)
            {
                currentOrientation = SnakeOrientation.Left;

                socket.SendChangeSnakeOrientationRequest(gameRoomID, SnakeOrientation.Left);
            }
        }
    }
}
