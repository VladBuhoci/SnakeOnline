using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnlineCore
{
    public class Snake
    {
        private SnakeOrientation currentOrientation;
        private Queue<SnakeOrientation> orientationQueue;

        private int bodyLength;
        private int bodyLengthMin;

        private Queue<SnakeBodyObject> bodyParts;

        private int bodyPartsToGrow;

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
            this.orientationQueue = new Queue<SnakeOrientation>();

            this.bodyLength = bodyLength;
            this.bodyLengthMin = bodyLength;

            this.bodyParts = BuildSnake(posX, posY, color);

            this.bodyPartsToGrow = 0;

            this.bIsAlive = true;
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

        public Queue<SnakeBodyObject> GetSnakeBodyParts()
        {
            return bodyParts;
        }

        // ~ Begin movement interface.

        public void ChangeDirection(SnakeOrientation newOrientation)
        {
            orientationQueue.Enqueue(newOrientation);
        }

        public void MoveSnake()
        {
            if (! bIsAlive)
                return;

            if (orientationQueue.Count > 0)
                currentOrientation = orientationQueue.Dequeue();

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

                    // Exit from this method.. no need to move anymore.
                    return;
                }
                else if (gameArenaObj is FoodObject)
                {
                    // Eat the food and grow some parts.

                    FoodObject foundFood = (FoodObject) gameArenaObj;

                    bodyPartsToGrow = foundFood.bodyPartsAmount;

                    SnakeGameManager.GetInstance().FoodWasEaten(foundFood);
                }
            }

            // We move the last body part up front, make it the new head and change the previous head to a simple part.
            // The new head also needs some changes regarding the position in the arena matrix, relative to the old head.

            SnakeBodyObject bodyPartToBecomeNewHead;

            if (bodyPartsToGrow > 0)
            {
                bodyPartToBecomeNewHead = new SnakeBodyObject(newHeadPosX, newHeadPosY, true, bodyParts.Last().color, this);

                bodyPartsToGrow -= 1;
            }
            else
            {
                bodyPartToBecomeNewHead = bodyParts.Dequeue();

                // Do the same changes in the arena matrix.
                SnakeGameManager.GetInstance().gameArenaObjects[bodyPartToBecomeNewHead.posX, bodyPartToBecomeNewHead.posY] = null;

                bodyPartToBecomeNewHead.isHead = true;
                bodyPartToBecomeNewHead.posX = newHeadPosX;
                bodyPartToBecomeNewHead.posY = newHeadPosY;
            }

            bodyParts.Last().isHead = false;

            // Same for arena matrix.. just to be sure nothing bad happens.
            ((SnakeBodyObject) SnakeGameManager.GetInstance().gameArenaObjects[bodyParts.Last().posX, bodyParts.Last().posY]).isHead = false;

            bodyParts.Enqueue(bodyPartToBecomeNewHead);

            // Same for the arena matrix.
            SnakeGameManager.GetInstance().gameArenaObjects[bodyPartToBecomeNewHead.posX, bodyPartToBecomeNewHead.posY] = bodyPartToBecomeNewHead;
        }

        // ~ End movement interface.
    }
}