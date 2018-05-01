using System;

namespace SnakeOnlineCore
{
    /// <summary>
    ///     Snake Online Communication Protocol.
    /// </summary>
    public enum Socp : Int32
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
        ///     Used when a client attempts to open a new game room.
        ///     A game manager is spawned on the server.
        ///     The server will create a new manager and send its ID back to the room leader (client) along with other room info.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_GAME_ROOM_CREATION = 4,

        /// <summary>
        ///     Used when the client joins a match and it requires a snake to take control of during gameplay.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        SPAWN_SNAKE = 5,

        /// <summary>
        ///     Used when a player wants to leave a game room and return to the lobby.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_DISCONNECT_FROM_GAME_ROOM = 6,

        /// <summary>
        ///     Used when the client closes the program and thus, disconnects from the server.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_DISCONNECT_FROM_SERVER = 7,


        // ===================================================================================================


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
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SERVER_BROADCAST_NEW_CHAT_MESSAGE_LOBBY = 103,

        /// <summary>
        ///     Used whenever a new game (and a game manager) is created .
        ///     Every manager needs to be identified by using a unique ID.
        ///     This ID, along with all the important room/game info is sent back to the client that created the room.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        GAME_ROOM_REQUEST_ACCEPTED = 104,

        /// <summary>
        ///     Used whenever the arena matrix is changed in some way and the clients need to be aware of that change.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_ARENA_MATRIX = 105,

        /// <summary>
        ///     Lets the user know that the server has accepted their request of leaving a room.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        RESPONSE_DISCONNECT_FROM_GAME_ROOM = 106,

        /// <summary>
        ///     The server is ready to release all resources and the client's application can close.
        ///     <para/>
        ///     TECH NOTE: this will not use the regular flow of callback functions in the server's socket wrapper class.
        ///                Instead, it will use an asynchronous Send method, so when it is done, the server knows that
        ///                it can close the client's socket and release used resources.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        RESPONSE_DISCONNECT_FROM_SERVER = 107
    }
}