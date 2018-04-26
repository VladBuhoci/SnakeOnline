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
        ///     Used in the beginning, when the player wants to be accepted by the server and join the lobby.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        CONNECT_TO_LOBBY_WITH_NICNKNAME = 1,

        /// <summary>
        ///     Used by the clients to get a new list of users currently connected to the server AND who joined the lobby.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_LOBBY_PEOPLE_LIST_UPDATE = 2,

        /// <summary>
        ///     Used when a regular client types and sends a text message in the lobby chat.
        /// </summary>
        CLIENT_POST_NEW_CHAT_MESSAGE_LOBBY = 3,

        /// <summary>
        ///     Used when a client attempts to start a new game and a game manager is spawned.
        ///     This will cause the server to create a new manager and send an ID back to the client(s).
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        CREATE_GAME = 4,

        /// <summary>
        ///     Used when the client joins a match and it requires a snake to take control of during gameplay.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        SPAWN_SNAKE = 5,


        // Server to client "commands".

        /// <summary>
        ///     Used by the server to send a temporary identifier for the newly connected client.
        ///     When joining the lobby, this client has to provide a nickname which will
        ///         replace the previously assigned id.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_CLIENT_TEMP_UNIQUE_ID = 100,

        /// <summary>
        ///     Used when the player joins the lobby and provides a unique name for themselves.
        ///     This will trigger the opening of the lobby window for the player if the server's response 
        ///         is their chosen nickname and not an empty string.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        ACCEPT_NEW_CLIENT_WITH_NICKNAME = 101,

        /// <summary>
        ///     Used by the server to inform all clients about the change(s) which occured recently in the
        ///         collection of clients, such as a new user who has just connected or somebody who left.
        ///     <para/>
        ///     Good for updating the list of people in the lobby of each client.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_CONNECTED_CLIENTS_COLLECTION = 102,

        /// <summary>
        ///     Used when the server sends a text message to all clients in the lobby chat.
        /// </summary>
        SERVER_BROADCAST_NEW_CHAT_MESSAGE_LOBBY = 103,

        /// <summary>
        ///     Used whenever a new game is going to be created and a game manager is born.
        ///     Every manager needs to be identified by using a unique ID.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_GAME_MANAGER_ID = 104,

        /// <summary>
        ///     Used whenever the arena matrix is changed in some way and the clients need to be aware of that change.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_ARENA_MATRIX = 105
    }
}