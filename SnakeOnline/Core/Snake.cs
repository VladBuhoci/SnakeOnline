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

        private bool _bIsAlive;     // real field.
        public bool bIsAlive
        {
            get
            {
                return _bIsAlive;
            }

            set
            {
                _bIsAlive = value;

                // 'false' means death.
                if (value == false)
                {
                    // Change the color of each body part to let the player know they've lost.
                    foreach (SnakeBodyObject snakePart in bodyParts)
                    {
                        snakePart.color = Color.Gray;
                    }
                }
            }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Snake(int posX, int posY, Color color, SnakeOrientation snakeOrientation = SnakeOrientation.Right, int bodyLength = 4)
        {
            this.currentOrientation = snakeOrientation;

            this.bodyLength = bodyLength;
            this.bodyLengthMin = bodyLength;

            this.bodyParts = BuildSnake(posX, posY, color);

            bIsAlive = true;

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
                        snakeParts.Enqueue(new SnakeBodyObject(posX, posY - i, false, color, this));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX, posY - bodyLength, true, color, this));   // The head.

                    break;

                case SnakeOrientation.Right:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX + i, posY, false, color, this));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX + bodyLength, posY, true, color, this));   // The head.

                    break;

                case SnakeOrientation.Down:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX, posY + i, false, color, this));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX, posY + bodyLength, true, color, this));   // The head.

                    break;

                case SnakeOrientation.Left:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX - i, posY, false, color, this));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX - bodyLength, posY, true, color, this));   // The head.

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
            if (! bIsAlive)
                return;

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

            if (newHeadPosX >= SnakeGameManager.GetInstance().gameArenaWidth)
                newHeadPosX = 0;
            else if (newHeadPosX < 0)
                newHeadPosX = SnakeGameManager.GetInstance().gameArenaWidth - 1;

            if (newHeadPosY >= SnakeGameManager.GetInstance().gameArenaHeight)
                newHeadPosY = 0;
            else if (newHeadPosY < 0)
                newHeadPosY = SnakeGameManager.GetInstance().gameArenaHeight - 1;

            // Before proceeding to move the snake pieces, check if there's another thing sitting in that place already.

            SnakeGameArenaObject gameArenaObj;
            if ((gameArenaObj = SnakeGameManager.GetInstance().gameArenaObjects[newHeadPosX, newHeadPosY]) != null)
            {
                if (gameArenaObj is SnakeBodyObject)
                {
                    // Snake dies when hitting another snake (or itself)
                    SnakeGameManager.GetInstance().KillSnake(this);

                    // See if it has hit another snake head, in which case both snakes die.
                    if (((SnakeBodyObject) gameArenaObj).isHead)
                    {
                        SnakeGameManager.GetInstance().KillSnake(((SnakeBodyObject) gameArenaObj).snake);
                    }
                }

                return;
            }

            // We move the last body part up front, make it the new head and change the previous head to a simple part.
            // The new head also needs some changes regarding the position in the arena matrix, relative to the old head.

            var oldTailToBecomeNewHead = bodyParts.Dequeue();

            // Do the same changes in the arena matrix.
            SnakeGameManager.GetInstance().gameArenaObjects[oldTailToBecomeNewHead.posX, oldTailToBecomeNewHead.posY] = null;

            oldTailToBecomeNewHead.isHead = true;
            oldTailToBecomeNewHead.posX = newHeadPosX;
            oldTailToBecomeNewHead.posY = newHeadPosY;

            bodyParts.Last().isHead = false;

            // Same for arena matrix.. just to be sure nothing bad happens.
            ((SnakeBodyObject) SnakeGameManager.GetInstance().gameArenaObjects[bodyParts.Last().posX, bodyParts.Last().posY]).isHead = false;

            bodyParts.Enqueue(oldTailToBecomeNewHead);

            // Same for the arena matrix.
            SnakeGameManager.GetInstance().gameArenaObjects[oldTailToBecomeNewHead.posX, oldTailToBecomeNewHead.posY] = oldTailToBecomeNewHead;
        }

        // ~ End movement interface.
    }
}
