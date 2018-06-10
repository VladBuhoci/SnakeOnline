using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeOnlineServer
{
    class SnakeGameManagerSV : SnakeGameManager
    {
        private Dictionary<string, Snake> playerWithSnakeCollection;
        private List<string> spectatorList;

        private SnakeGameDescriptor gameDescriptor;

        private GameServer gameServer;
        
        public SnakeGameManagerSV(GameServer server, SnakeGameDescriptor descriptor)
            : base(descriptor.arenaWidth, descriptor.arenaHeight)
        {
            gameArenaObjects = new SnakeGameArenaObject[descriptor.arenaWidth, descriptor.arenaHeight];
            playerWithSnakeCollection = new Dictionary<string, Snake>();
            spectatorList = new List<string>();

            gameDescriptor = descriptor;

            gameServer = server;
            
            // Add the leader to the Players With Snakes collection automatically.
            playerWithSnakeCollection.Add(descriptor.roomLeaderID, null);
        }

        public void StartGame()
        {
            gameDescriptor.roomState = GameRoomState.PLAYING;

            // Create and add the snakes in the arena.

            foreach (String player in Players)
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                int randPosX, randPosY;
                SnakeOrientation randOrientation;

                // Generate a random position that is suitable for the new snake.

                do
                {
                    List<SnakeOrientation> orientationOptions = new List<SnakeOrientation>();

                    randPosX = rand.Next(0, gameArenaWidth);
                    randPosY = rand.Next(0, gameArenaHeight);
                    
                    if (gameArenaObjects[randPosX, randPosY] != null)
                    {
                        continue;
                    }

                    // See if the snake can be placed here, oriented to the right, left, up or down.
                    
                    // RIGHT:
                    if (randPosX + 3 < gameArenaWidth
                        && gameArenaObjects[randPosX + 1, randPosY] == null
                        && gameArenaObjects[randPosX + 2, randPosY] == null
                        && gameArenaObjects[randPosX + 3, randPosY] == null)
                    {
                        orientationOptions.Add(SnakeOrientation.Right);
                    }

                    // LEFT
                    if (randPosX - 3 >= 0
                        && gameArenaObjects[randPosX - 1, randPosY] == null
                        && gameArenaObjects[randPosX - 2, randPosY] == null
                        && gameArenaObjects[randPosX - 3, randPosY] == null)
                    {
                        orientationOptions.Add(SnakeOrientation.Left);
                    }

                    // UP
                    if (randPosY - 3 >= 0
                        && gameArenaObjects[randPosX, randPosY - 1] == null
                        && gameArenaObjects[randPosX, randPosY - 2] == null
                        && gameArenaObjects[randPosX, randPosY - 3] == null)
                    {
                        orientationOptions.Add(SnakeOrientation.Up);
                    }

                    // DOWN
                    if (randPosY + 3 < gameArenaHeight
                        && gameArenaObjects[randPosX, randPosY + 1] == null
                        && gameArenaObjects[randPosX, randPosY + 2] == null
                        && gameArenaObjects[randPosX, randPosY + 3] == null)
                    {
                        orientationOptions.Add(SnakeOrientation.Down);
                    }

                    // If there were no valid positions, generate a new pair of coordinates.
                    if (orientationOptions.Count == 0)
                    {
                        continue;
                    }

                    // Pick one of the valid orientations.
                    randOrientation = orientationOptions[rand.Next(0, orientationOptions.Count())];

                    // We found a solution, so we can move on.
                    break;
                }
                while (true);

                // Create the snake with the generated values.
                Snake snake = new Snake(randPosX, randPosY, Color.Red, player, randOrientation);

                // Add it to the game.
                AddSnake(snake);

                // Ask the player to initialize a local controller.
                gameServer.SendStartGameRequestToClient(player, randOrientation);
            }

            // Ask every spectator to initialize a local controller.
            foreach (string spec in Spectators)
            {
                gameServer.SendStartGameRequestToClient(spec, null);
            }

            // Start the game.
            StartGameLoop();
        }

        protected override void GameLoop(SnakeGameManager gameManager, List<Snake> snakeList)
        {
            Thread.Sleep(TIME_BEFORE_GAME_STARTS * 1000);

            while (true)
            {
                if (bApplicationAttemptsClosing)
                {
                    break;
                }

                foreach (Snake snake in snakeList)
                {
                    snake.MoveSnake(gameManager);
                }

                gameServer.SendUpdatedArenaDataToClients(gameDescriptor.gameManagerID, gameArenaObjects);

                Thread.Sleep(100);
            }

            Thread.CurrentThread.Abort();
        }

        public override void KillSnake(string snakeID)
        {
            foreach (Snake snake in snakes)
            {
                if (String.Equals(snake.GetID(), snakeID))
                {
                    snake.bIsAlive = false;

                    // other things like notifying the player or something...

                    break;
                }
            }
        }

        public void ChangeSnakeOrientation(string playerID, SnakeOrientation newOrientation)
        {
            foreach (Snake snake in snakes)
            {
                if (String.Equals(snake.GetID(), playerID))
                {
                    snake.ChangeOrientation(newOrientation);
                    
                    break;
                }
            }
        }

        public void SwitchSidesForClient(string clientID)
        {
            if (playerWithSnakeCollection.Keys.Contains(clientID))
            {
                playerWithSnakeCollection.Remove(clientID);
                gameDescriptor.currentPlayerCount -= 1;

                spectatorList.Add(clientID);
                gameDescriptor.currentSpectatorCount += 1;
            }
            else if (spectatorList.Contains(clientID))
            {
                spectatorList.Remove(clientID);
                gameDescriptor.currentSpectatorCount -= 1;

                playerWithSnakeCollection.Add(clientID, null);
                gameDescriptor.currentPlayerCount += 1;
            }

            gameServer.BroadcastPlayerListForRoom(gameDescriptor.gameManagerID);
            gameServer.BroadcastSpectatorListForRoom(gameDescriptor.gameManagerID);
            gameServer.BroadcastRoomListForLobby();
        }

        public void AddClientToGameRoom(string clientID)
        {
            if (gameDescriptor.roomState == GameRoomState.WAITING && gameDescriptor.currentPlayerCount < gameDescriptor.maxSnakesAllowed)
            {
                playerWithSnakeCollection.Add(clientID, null);
                gameDescriptor.currentPlayerCount += 1;

                gameServer.BroadcastPlayerListForRoom(gameDescriptor.gameManagerID);
            }
            else
            {
                spectatorList.Add(clientID);
                gameDescriptor.currentSpectatorCount += 1;

                gameServer.BroadcastSpectatorListForRoom(gameDescriptor.gameManagerID);
            }
        }

        public void RemoveClientFromGameRoom(string clientID)
        {
            if (playerWithSnakeCollection.Keys.Contains(clientID))
            {
                // TODO: handle this player's now-orphan snake.
                {

                }

                playerWithSnakeCollection.Remove(clientID);
                gameDescriptor.currentPlayerCount -= 1;

                if (! IsGameEmpty())
                {
                    gameServer.BroadcastPlayerListForRoom(gameDescriptor.gameManagerID);
                }
            }
            else if (spectatorList.Contains(clientID))
            {
                spectatorList.Remove(clientID);
                gameDescriptor.currentSpectatorCount -= 1;

                if (! IsGameEmpty())
                {
                    gameServer.BroadcastSpectatorListForRoom(gameDescriptor.gameManagerID);
                }
            }

            if (! IsGameEmpty())
            {
                // If the removed client was the room's leader, find a new one.
                if (String.Equals(clientID, gameDescriptor.roomLeaderID))
                {
                    if (Players.Length > 0)
                    {
                        gameDescriptor.roomLeaderID = Players[0];
                    }
                    else //if (Spectators.Length > 0)
                    {
                        gameDescriptor.roomLeaderID = Spectators[0];
                    }
                }

                gameServer.SendGameRoomLeaderHasChangedMessageToClients(gameDescriptor.gameManagerID, gameDescriptor.roomLeaderID);
            }
        }

        public SnakeGameDescriptor GetDescriptor()
        {
            return gameDescriptor;
        }

        /// <summary>
        ///     Returns true if there are no players left in this game room.
        /// </summary>
        public bool IsGameEmpty()
        {
            return gameDescriptor.currentPlayerCount <= 0 && gameDescriptor.currentSpectatorCount <= 0;
        }

        public string[] Players
        {
            get
            {
                return playerWithSnakeCollection.Keys.ToArray();
            }
        }

        public string[] Spectators
        {
            get
            {
                return spectatorList.ToArray();
            }
        }
    }
}