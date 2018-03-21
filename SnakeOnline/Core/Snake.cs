using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Core
{
    class Snake
    {
        private SnakeOrientation currentOrientation;

        private int bodyLength;
        private int bodyLengthMin;

        private Queue<SnakeBodyObject> bodyParts;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Snake(int posX, int posY, Color color, SnakeOrientation snakeOrientation = SnakeOrientation.Right, int bodyLength = 4)
        {
            this.currentOrientation = snakeOrientation;

            this.bodyLength = bodyLength;
            this.bodyLengthMin = bodyLength;

            this.bodyParts = BuildSnake(posX, posY, color);

            SnakeGameManager.GetInstance().AddSnake(this, bodyParts);
        }

        private Queue<SnakeBodyObject> BuildSnake(int posX, int posY, Color color)
        {
            Queue<SnakeBodyObject> snakeParts = new Queue<SnakeBodyObject>();

            switch (currentOrientation)
            {
                case SnakeOrientation.Up:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX, posY - i, false, color));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX, posY - bodyLength, true, color));   // The head.

                    break;

                case SnakeOrientation.Right:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX + i, posY, false, color));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX + bodyLength, posY, true, color));   // The head.

                    break;

                case SnakeOrientation.Down:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX, posY + i, false, color));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX, posY + bodyLength, true, color));   // The head.

                    break;

                case SnakeOrientation.Left:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX - i, posY, false, color));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX - bodyLength, posY, true, color));   // The head.

                    break;
            }            

            return snakeParts;
        }


        // ~ Begin movement interface.

        public void ChangeDirection(SnakeOrientation newOrientation)
        {
            currentOrientation = newOrientation;
        }

        public void MoveSnake()
        {
            switch (currentOrientation)
            {
                case SnakeOrientation.Up:
                    MoveSnakeInCurrentDirection(bodyParts.Last().posX, bodyParts.Last().posY - 1);
                    break;

                case SnakeOrientation.Right:
                    MoveSnakeInCurrentDirection(bodyParts.Last().posX + 1, bodyParts.Last().posY);
                    break;

                case SnakeOrientation.Down:
                    MoveSnakeInCurrentDirection(bodyParts.Last().posX, bodyParts.Last().posY + 1);
                    break;

                case SnakeOrientation.Left:
                    MoveSnakeInCurrentDirection(bodyParts.Last().posX - 1, bodyParts.Last().posY);
                    break;
            }
        }

        private void MoveSnakeInCurrentDirection(int newHeadPosX, int newHeadPosY)
        {
            // Handle the new coordinates first:
            //      they may make the snake go out of bounds, so try to switch them with the values on the
            //      opposite side of the arena matrix, giving the illusion of "portals" in the walls.

            if (newHeadPosX >= SnakeGameManager.gameArenaWidth)
                newHeadPosX = 0;
            else if (newHeadPosX < 0)
                newHeadPosX = SnakeGameManager.gameArenaWidth;

            if (newHeadPosY >= SnakeGameManager.gameArenaHeight)
                newHeadPosY = 0;
            else if (newHeadPosY < 0)
                newHeadPosY = SnakeGameManager.gameArenaHeight;

            // We move the last body part up front, make it the new head and change the previous head to a simple part.
            // The new head also needs some changes regarding the position in the arena matrix, relative to the old head.

            var oldTailToBecomeNewHead = bodyParts.Dequeue();

            oldTailToBecomeNewHead.isHead = true;
            oldTailToBecomeNewHead.posX = newHeadPosX;
            oldTailToBecomeNewHead.posY = newHeadPosY;

            bodyParts.Last().isHead = false;

            bodyParts.Enqueue(oldTailToBecomeNewHead);
        }

        // ~ End movement interface.
    }
}
