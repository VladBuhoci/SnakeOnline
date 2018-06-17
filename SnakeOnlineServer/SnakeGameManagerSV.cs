using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeOnlineServer
{
    class SnakeGameManagerSV : SnakeGameManager
    {
        private Dictionary<string, Color> playerWithSnakeCollection;
        private List<string> spectatorList;
        private List<Color> colors;
        private int powerUpSpawnTimer;

        private SnakeGameDescriptor gameDescriptor;

        private GameServer gameServer;
        
        public SnakeGameManagerSV(GameServer server, SnakeGameDescriptor descriptor)
            : base(descriptor.arenaWidth, descriptor.arenaHeight)
        {
            gameArenaObjects = new SnakeGameArenaObject[descriptor.arenaWidth, descriptor.arenaHeight];
            playerWithSnakeCollection = new Dictionary<string, Color>();
            spectatorList = new List<string>();

            gameDescriptor = descriptor;

            gameServer = server;
            
            // Add the leader to the Players With Snakes collection automatically.
            playerWithSnakeCollection.Add(descriptor.roomLeaderID, Color.Aqua);

            colors = new List<Color>();
            PropertyInfo[] colorsInfo = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo colorInfo in colorsInfo)
            {
                colors.Add((Color) colorInfo.GetValue(colorInfo));
            }
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
                Snake snake = new Snake(randPosX, randPosY, playerWithSnakeCollection[player], player, randOrientation);

                // Add it to the game.
                AddSnake(snake);

                SpawnFood(playerWithSnakeCollection[player]);

                // Ask the player to initialize a local controller.
                gameServer.SendStartGameRequestToClient(player, randOrientation);
            }

            // Ask every spectator to initialize a local controller.
            foreach (string spec in Spectators)
            {
                gameServer.SendStartGameRequestToClient(spec, SnakeOrientation.None);
            }

            gameServer.SendUpdatedArenaDataToClients(gameDescriptor.gameManagerID, gameArenaObjects, gameDescriptor.matchDuration * 60);

            // Start the game.
            StartGameLoop();
        }

        protected override void GameLoop(SnakeGameManager gameManager, List<Snake> snakeList)
        {
            int gameDuration = gameDescriptor.matchDuration * 60000;

            powerUpSpawnTimer = new Random(DateTime.Now.Millisecond).Next(5, 10) * 1000;        // turn this into seconds.

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
                    // Have the snakes under the effect of the Speed Power Up move more times.
                    for (int k = 0; k < snakeList[i].SpeedAmplifier; k += 1)
                    {
                        snakeList[i].MoveSnake(gameManager);
                    }
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

                powerUpSpawnTimer -= 100;
                if (powerUpSpawnTimer <= 0)
                {
                    SpawnPowerUp(GenerateRandomColour());
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
                    snake.IsAlive = false;
                    
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
                    snake.IsAlive = false;

                    // other things like notifying the player or something...
                }
            }

            CheckIfGameShouldEnd();
        }

        public override void SpawnFood(Color color)
        {
            int randPosX, randPosY, randAmount;
            Random random = new Random(DateTime.Now.Millisecond);

            randAmount = random.Next(1, 4);

            do
            {
                randPosX = random.Next(0, gameArenaWidth);
                randPosY = random.Next(0, gameArenaHeight);

                if (gameArenaObjects[randPosX, randPosY] != null)
                    continue;

                break;
            }
            while (true);

            FoodObject food = new FoodObject(randPosX, randPosY, color, randAmount);

            AddFood(food);
        }

        public override void SpawnPowerUp(Color color)
        {
            int randPosX, randPosY;
            Random random = new Random(DateTime.Now.Millisecond);
            
            do
            {
                randPosX = random.Next(0, gameArenaWidth);
                randPosY = random.Next(0, gameArenaHeight);

                if (gameArenaObjects[randPosX, randPosY] != null)
                    continue;

                break;
            }
            while (true);

            // TODO: add some variety perhaps?
            PowerUpObject powerUp = new SpeedPowerUpObject(randPosX, randPosY, color, random.Next(2, 5), random.Next(2, 5));

            AddPowerUp(powerUp);

            powerUpSpawnTimer = random.Next(3, 10) * 1000;      // turn the number into seconds.
        }

        private void CheckIfGameShouldEnd()
        {
            int aliveSnakes = 0;

            foreach (Snake snake in snakes)
            {
                if (snake.IsAlive)
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

                playerWithSnakeCollection.Add(clientID, GenerateRandomColour());
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
                playerWithSnakeCollection.Add(clientID, GenerateRandomColour());
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
                foreach (Snake snk in snakes)
                {
                    if (String.Equals(snk.GetID(), clientID))
                    {
                        RemoveSnake(snk);

                        CheckIfGameShouldEnd();

                        break;
                    }
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
                        resultMessage = String.Format("The winner is: {0} !", snakes.Where(snk => snk.IsAlive).First().GetID());

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
                        snakes = snakes.OrderBy(snk => snk.GetSnakeBodyParts().Count).ToList();

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

        public void ChangeSnakeColour(string playerID, Color newColour)
        {
            if (playerWithSnakeCollection.Keys.Contains(playerID))
            {
                playerWithSnakeCollection[playerID] = newColour;

                gameServer.BroadcastPlayerListForRoom(gameDescriptor.gameManagerID);
            }
        }

        private Color GenerateRandomColour()
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            return colors[rand.Next(0, colors.Count)];
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

        public string[] PlayersWithColorNames
        {
            get
            {
                List<string> playersWithColors = new List<string>();

                foreach (string player in playerWithSnakeCollection.Keys)
                {
                    string colour = playerWithSnakeCollection[player].Name.ToUpper();

                    playersWithColors.Add(String.Format("{0} ({1})", player, colour));
                }

                return playersWithColors.ToArray();
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