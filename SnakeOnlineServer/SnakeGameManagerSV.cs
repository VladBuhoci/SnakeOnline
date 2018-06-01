﻿using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnlineServer
{
    class SnakeGameManagerSV
    {
        private bool bApplicationAttemptsClosing;
        
        private SnakeGameArenaObject[,] gameArenaObjects;
        private Dictionary<string, Snake> playerWithSnakeCollection;
        private List<string> spectatorList;

        private SnakeGameDescriptor gameDescriptor;

        private GameServer gameServer;

        //private string roomLeaderID;

        public SnakeGameManagerSV(GameServer server/*, int managerID*/, SnakeGameDescriptor descriptor)
        {
            bApplicationAttemptsClosing = false;
            
            gameArenaObjects = new SnakeGameArenaObject[descriptor.arenaWidth, descriptor.arenaHeight];
            playerWithSnakeCollection = new Dictionary<string, Snake>();
            spectatorList = new List<string>();

            gameDescriptor = descriptor;

            gameServer = server;
            
            // Add the leader to the Players With Snakes collection automatically.
            playerWithSnakeCollection.Add(descriptor.roomLeaderID, null);
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

            gameServer.BroadcastRoomListForLobby();
        }

        public void RemoveClientFromGameRoom(string clientID)
        {
            if (playerWithSnakeCollection.Keys.Contains(clientID))
            {
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
                // TODO: more actions to be handled here, including a check whether this
                //          was the room leader, in which case we have to find a new leader.
                //          If there are no players left, perhaps ask the server to kill the room.

                
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

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }
    }
}