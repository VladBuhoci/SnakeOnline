using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeOnlineCore
{
    [Serializable]
    public class Snake
    {
        private string snakeID;

        private SnakeOrientation currentOrientation;
        private Queue<SnakeOrientation> orientationQueue;

        private int bodyLength;
        private int bodyLengthMin;

        private Queue<SnakeBodyObject> bodyParts;

        private int bodyPartsToGrow;

        private bool bIsAlive;
        private int speedAmplifier;

        public bool IsAlive
        {
            get
            {
                return bIsAlive;
            }

            set
            {
                bIsAlive = value;

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

        public int SpeedAmplifier
        {
            get
            {
                return speedAmplifier;
            }
        }

        public string GetID()
        {
            return snakeID;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Snake(int posX, int posY, Color color, string snakeID, SnakeOrientation snakeOrientation = SnakeOrientation.Right, int bodyLength = 4)
        {
            this.snakeID = snakeID;

            this.currentOrientation = snakeOrientation;
            this.orientationQueue = new Queue<SnakeOrientation>();

            this.bodyLength = bodyLength;
            this.bodyLengthMin = bodyLength;

            this.bodyParts = BuildSnake(posX, posY, color);

            this.bodyPartsToGrow = 0;

            this.bIsAlive = true;
            this.speedAmplifier = 1;
        }

        private Queue<SnakeBodyObject> BuildSnake(int posX, int posY, Color color)
        {
            Queue<SnakeBodyObject> snakeParts = new Queue<SnakeBodyObject>();

            switch (currentOrientation)
            {
                case SnakeOrientation.Up:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX, posY - i, false, color, snakeID));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX, posY - bodyLength, true, color, snakeID));   // The head.

                    break;

                case SnakeOrientation.Right:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX + i, posY, false, color, snakeID));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX + bodyLength, posY, true, color, snakeID));   // The head.

                    break;

                case SnakeOrientation.Down:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX, posY + i, false, color, snakeID));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX, posY + bodyLength, true, color, snakeID));   // The head.

                    break;

                case SnakeOrientation.Left:
                    for (int i = 0; i < bodyLength; i++)
                    {
                        snakeParts.Enqueue(new SnakeBodyObject(posX - i, posY, false, color, snakeID));
                    }

                    snakeParts.Enqueue(new SnakeBodyObject(posX - bodyLength, posY, true, color, snakeID));   // The head.

                    break;
            }
            
            return snakeParts;
        }

        public Queue<SnakeBodyObject> GetSnakeBodyParts()
        {
            return bodyParts;
        }

        public SnakeOrientation GetOrientation()
        {
            return currentOrientation;
        }

        // ~ Begin movement interface.

        public void ChangeOrientation(SnakeOrientation newOrientation)
        {
            orientationQueue.Enqueue(newOrientation);
        }

        public void MoveSnake(SnakeGameManager gameManager)
        {
            if (! IsAlive)
                return;

            if (orientationQueue.Count > 0)
                currentOrientation = orientationQueue.Dequeue();

            switch (currentOrientation)
            {
                case SnakeOrientation.Up:
                    MoveSnakeInCurrentDirection(gameManager, bodyParts.Last().posX, bodyParts.Last().posY - 1);
                    break;

                case SnakeOrientation.Right:
                    MoveSnakeInCurrentDirection(gameManager, bodyParts.Last().posX + 1, bodyParts.Last().posY);
                    break;

                case SnakeOrientation.Down:
                    MoveSnakeInCurrentDirection(gameManager, bodyParts.Last().posX, bodyParts.Last().posY + 1);
                    break;

                case SnakeOrientation.Left:
                    MoveSnakeInCurrentDirection(gameManager, bodyParts.Last().posX - 1, bodyParts.Last().posY);
                    break;
            }
        }

        private void MoveSnakeInCurrentDirection(SnakeGameManager gameManager, int newHeadPosX, int newHeadPosY)
        {
            // Handle the new coordinates first:
            //      they may make the snake go out of bounds, so try to switch them with the values on the
            //      opposite side of the arena matrix, giving the illusion of "portals" in the walls.

            if (newHeadPosX >= gameManager.gameArenaWidth)
                newHeadPosX = 0;
            else if (newHeadPosX < 0)
                newHeadPosX = gameManager.gameArenaWidth - 1;

            if (newHeadPosY >= gameManager.gameArenaHeight)
                newHeadPosY = 0;
            else if (newHeadPosY < 0)
                newHeadPosY = gameManager.gameArenaHeight - 1;

            // Before proceeding to move the snake pieces, check if there's another thing sitting in that place at the moment.

            SnakeGameArenaObject gameArenaObj;
            if ((gameArenaObj = gameManager.gameArenaObjects[newHeadPosX, newHeadPosY]) != null)
            {
                if (gameArenaObj is SnakeBodyObject otherSnakeBodyObj)
                {
                    // See if it has hit another snake's head, in which case both snakes die.
                    if (otherSnakeBodyObj.isHead && IsHeadOnHeadFight(otherSnakeBodyObj))
                    {
                        gameManager.KillSnakes(this.snakeID, otherSnakeBodyObj.snakeID);
                    }
                    else
                    {
                        // Snake dies when hitting another snake (or itself)
                        gameManager.KillSnake(this.snakeID);
                    }

                    // Exit now.. no need to move anymore.
                    return;
                }
                else if (gameArenaObj is FoodObject foundFood)
                {
                    // Eat the food and grow some parts.

                    if (foundFood.color == bodyParts.Peek().color)
                    {
                        bodyPartsToGrow = foundFood.bodyPartsAmount;
                    }

                    gameManager.FoodWasEaten(foundFood);
                }
                else if (gameArenaObj is PowerUpObject powerUp)
                {
                    if (powerUp is SpeedPowerUpObject speedPowerUp)
                    {
                        speedAmplifier = speedPowerUp.speedAmplifier;

                        Thread speedStopperThread = new Thread(() =>
                        {
                            Thread.Sleep(speedPowerUp.effectDuration * 1000);

                            speedAmplifier = 1;

                            Thread.CurrentThread.Abort();
                        });
                        speedStopperThread.Start();
                    }
                }
            }

            // We move the last body part up front, make it the new head and change the previous head to a simple part.
            // The new head also needs some changes regarding the position in the arena matrix, relative to the old head.

            SnakeBodyObject bodyPartToBecomeNewHead;

            if (bodyPartsToGrow > 0)
            {
                bodyPartToBecomeNewHead = new SnakeBodyObject(newHeadPosX, newHeadPosY, true, bodyParts.Last().color, snakeID);

                bodyPartsToGrow -= 1;
            }
            else
            {
                bodyPartToBecomeNewHead = bodyParts.Dequeue();

                // Do the same changes in the arena matrix.
                gameManager.gameArenaObjects[bodyPartToBecomeNewHead.posX, bodyPartToBecomeNewHead.posY] = null;

                bodyPartToBecomeNewHead.isHead = true;
                bodyPartToBecomeNewHead.posX = newHeadPosX;
                bodyPartToBecomeNewHead.posY = newHeadPosY;
            }

            bodyParts.Last().isHead = false;

            // Same for arena matrix.. just to be sure nothing bad happens.
            ((SnakeBodyObject) gameManager.gameArenaObjects[bodyParts.Last().posX, bodyParts.Last().posY]).isHead = false;

            bodyParts.Enqueue(bodyPartToBecomeNewHead);

            // Same for the arena matrix.
            gameManager.gameArenaObjects[bodyPartToBecomeNewHead.posX, bodyPartToBecomeNewHead.posY] = bodyPartToBecomeNewHead;
        }

        // ~ End movement interface.

        private bool IsHeadOnHeadFight(SnakeBodyObject otherSnakeHead)
        {
            SnakeBodyObject thisSnakeHead = bodyParts.Last();

            // Check this snake's head position relative to the other snake's.

            // Left:
            if (thisSnakeHead.posX == otherSnakeHead.posX - 1
                && thisSnakeHead.posY == otherSnakeHead.posY)
            {
                return true;
            }

            // Right:
            else if (thisSnakeHead.posX == otherSnakeHead.posX + 1
                && thisSnakeHead.posY == otherSnakeHead.posY)
            {
                return true;
            }

            // Up:
            else if (thisSnakeHead.posX == otherSnakeHead.posX
                && thisSnakeHead.posY == otherSnakeHead.posY - 1)
            {
                return true;
            }

            // Down:
            else if (thisSnakeHead.posX == otherSnakeHead.posX
                && thisSnakeHead.posY == otherSnakeHead.posY + 1)
            {
                return true;
            }

            return false;
        }
    }
}