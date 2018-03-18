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

            SnakeGameManager.AddSnake(bodyParts);
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

        public void MoveUp()
        {
            if (currentOrientation != SnakeOrientation.Up)
            {
                currentOrientation = SnakeOrientation.Up;
            }
        }

        public void MoveRight()
        {
            if (currentOrientation != SnakeOrientation.Right)
            {
                currentOrientation = SnakeOrientation.Right;
            }
        }

        public void MoveDown()
        {
            if (currentOrientation != SnakeOrientation.Down)
            {
                currentOrientation = SnakeOrientation.Down;
            }
        }

        public void MoveLeft()
        {
            if (currentOrientation != SnakeOrientation.Left)
            {
                currentOrientation = SnakeOrientation.Left;
            }
        }

        // ~ End movement interface.
    }
}
