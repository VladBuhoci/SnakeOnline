using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnlineCore
{
    public enum CommunicationProtocol : Int32
    {
        // Client to server "commands".

        /// <summary>
        ///     Used when the client joins a match and it requires a snake to take control of during gameplay.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        SPAWN_SNAKE = 1,


        // Server to client "commands".

        /// <summary>
        ///     Used for when the player connects to the server and it generates a unique number for that player.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_PLAYER_ID = 11,

        /// <summary>
        ///     Used whenever the arena matrix is changed in some way and the clients need to be aware of that change.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_ARENA_MATRIX = 12
    }
}