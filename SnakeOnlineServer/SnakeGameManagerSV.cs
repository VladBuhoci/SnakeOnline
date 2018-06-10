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

            gameServer.SendUpdatedArenaDataToClients(gameDescriptor.gameManagerID, gameArenaObjects, gameDescriptor.matchDuration * 60);

            // Start the game.
            StartGameLoop();
        }

        protected override void GameLoop(SnakeGameManager gameManager, List<Snake> snakeList)
        {
            int gameDuration = gameDescriptor.matchDuration * 60000;

            Thread.Sleep(TIME_BEFORE_GAME_STARTS * 1000);

            while (true)
            {
                // Check if game loop is over.
                if (bApplicationAttemptsClosing)
                {
                    break;
                }

                for (int i = 0; i < snakeList.Count; i ++)
                {
                    snakeList[i].MoveSnake(gameManager);
                }

                // Check again.
                if (bApplicationAttemptsClosing)
                {
                    break;
                }

                gameServer.SendUpdatedArenaDataToClients(gameDescriptor.gameManagerID, gameArenaObjects, gameDuration / 1000);

                // Check again.
                if (bApplicationAttemptsClosing)
                {
                    break;
                }

                gameDuration -= 100;

                if (gameDuration <= 0)
                {
                    EndGame(GameOverType.TIME_OUT);
                }

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
                    
                    break;
                }
            }

            CheckIfGameShouldEnd();
        }

        public override void KillSnakes(params string[] snakeIDs)
        {
            foreach (Snake snake in snakes)
            {
                if (snakeIDs.Contains(snake.GetID()))
                {
                    snake.bIsAlive = false;

                    // other things like notifying the player or something...
                }
            }

            CheckIfGameShouldEnd();
        }

        private void CheckIfGameShouldEnd()
        {
            int aliveSnakes = 0;

            foreach (Snake snake in snakes)
            {
                if (snake.bIsAlive)
                {
                    aliveSnakes += 1;
                }
            }

            if (aliveSnakes == 0)
            {
                EndGame(GameOverType.ALL_DEAD);
            }
            else if (aliveSnakes == 1)
            {
                EndGame(GameOverType.LAST_ONE_STANDING);
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

        private void EndGame(GameOverType gameOverType)
        {
            if (gameDescriptor.roomState == GameRoomState.WAITING)
                return;

            // End the game loop.
            RequestAuxGameLoopThreadToEnd();

            // Draw the arena's state one more time.
            gameServer.SendUpdatedArenaDataToClients(gameDescriptor.gameManagerID, gameArenaObjects, 0);
            
            string resultMessage = "";

            switch (gameOverType)
            {
                case GameOverType.LAST_ONE_STANDING:
                    {
                        resultMessage = String.Format("The winner is: {0} !", snakes.Where(snk => snk.bIsAlive).First().GetID());

                        break;
                    }

                case GameOverType.ALL_DEAD:
                    {
                        resultMessage = "All snakes are dead. Nobody has won!";

                        break;
                    }

                case GameOverType.TIME_OUT:
                    {
                        // Prepare the leaderboard.
                        snakes.OrderBy(snk => snk.GetSnakeBodyParts().Count);

                        // See if there is a draw.

                        Snake[] winnerSnakes = snakes.Where(snk => snk.GetSnakeBodyParts().Count == snakes[0].GetSnakeBodyParts().Count).ToArray();
                        string[] winners = new string[winnerSnakes.Length];

                        for (int i = 0; i < winnerSnakes.Length; i++)
                        {
                            winners[i] = winnerSnakes[i].GetID();
                        }

                        if (winners.Length == 1)
                        {
                            resultMessage = String.Format("The winner is: {0} !", winners[0]);
                        }
                        else
                        {
                            resultMessage = String.Format("It is a draw between: {0} !", String.Join(", ", winners));
                        }

                        break;
                    }
            }

            Thread gameOverAndCleanUpThread = new Thread(() => GameOverAndCleanUp(resultMessage));
            gameOverAndCleanUpThread.Start();
        }

        private void GameOverAndCleanUp(string resultMessage)
        {
            Thread.Sleep(TIME_BEFORE_GAME_ENDS * 1000);

            // Broadcast the message.
            gameServer.SendGameOverResultToClients(gameDescriptor.gameManagerID, resultMessage, gameDescriptor.roomLeaderID);

            // Clear the data structures.
            // TODO: does it clear every row and/or column?
            Array.Clear(gameArenaObjects, 0, gameArenaObjects.Length);
            snakes.Clear();

            // Room is ready for a new match.
            gameDescriptor.roomState = GameRoomState.WAITING;

            Thread.CurrentThread.Abort();
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

        public string[] AllClients
        {
            get
            {
                return Players.Union(Spectators).ToArray();
            }
        }
    }
}