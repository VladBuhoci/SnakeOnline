using System;

namespace SnakeOnlineCore
{
    /// <summary>
    ///     Snake Online Communication Protocol message types.
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
        REQUEST_LOBBY_PEOPLE_LIST_UPDATE,

        /// <summary>
        ///     Used by the clients to get a new list of users who joined as players, currently connected to a specific room
        ///         (room/game manager ID must exist within the message object.)
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_ROOM_PLAYER_LIST_UPDATE,

        /// <summary>
        ///     Used by the clients to get a new list of users who joined as spectators, currently connected to a specific room
        ///         (room/game manager ID must exist within the message object.)
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_ROOM_SPECTATOR_LIST_UPDATE,

        /// <summary>
        ///     Used when a client wants to switch sides (from players to spectators or vice versa) in a room,
        ///         but only if there isn't an ongoing match linked to this room.
        ///         (room/game manager ID must exist within the message object.)
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_SWITCH_SIDES_ROOM,

        /// <summary>
        ///     Used when a regular client types and sends a text message in the lobby chat.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        CLIENT_POST_NEW_CHAT_MESSAGE_LOBBY,

        /// <summary>
        ///     Used when a regular client types and sends a text message in a specific room chat.
        ///         (room/game manager ID must exist within the message object.)
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        CLIENT_POST_NEW_CHAT_MESSAGE_ROOM,

        /// <summary>
        ///     Used when a client attempts to open a new game room.
        ///     A game manager is spawned on the server.
        ///     The server will create a new manager and send its ID back to the room leader (client) along with other room info.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_GAME_ROOM_CREATION,

        /// <summary>
        ///     Used when a client wants to see more information about a specific room.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_GAME_ROOM_DESCRIPTION,

        /// <summary>
        ///     Used when a client decides to join a room from the lobby list.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_GAME_ROOM_JOIN,

        /// <summary>
        ///     Used by the client when he/she needs to have an updated list of game rooms (in the lobby).
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_GAME_ROOM_COLLECTION_UPDATE,

        /// <summary>
        ///     Used when a client wants the game to begin in their room for every connected client.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_GAME_ROOM_MATCH_START,

        /// <summary>
        ///     Used when a player pressed a key to change the snake's orientation.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_CHANGE_SNAKE_ORIENTATION,

        /// <summary>
        ///     Used when a player wants to leave a game room and return to the lobby.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_DISCONNECT_FROM_GAME_ROOM,

        /// <summary>
        ///     Used when the client closes the program and thus, disconnects from the server.
        ///     <para/>
        ///     USAGE: client to server.
        /// </summary>
        REQUEST_DISCONNECT_FROM_SERVER,


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
        ACCEPT_NEW_CLIENT_WITH_NICKNAME,

        /// <summary>
        ///     Used by the server to inform all clients about the change(s) which occured recently in the
        ///         collection of clients, such as a new user who has just connected or somebody who left.
        ///     <para/>
        ///     Good for updating the list of people in the lobby of each client.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_CONNECTED_CLIENTS_COLLECTION,

        /// <summary>
        ///     Used by the server to inform all clients about the change(s) which occured recently in the
        ///         collection of players in a room, such as a new user who has just connected or somebody who left.
        ///     <para/>
        ///     Good for updating the list of players in a specific room window of each client.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_ROOM_PLAYER_COLLECTION,

        /// <summary>
        ///     Used by the server to inform all clients about the change(s) which occured recently in the
        ///         collection of spectators in a room, such as a new user who has just connected or somebody who left.
        ///     <para/>
        ///     Good for updating the list of spectators in a specific room window of each client.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_ROOM_SPECTATOR_COLLECTION,

        /// <summary>
        ///     Used when the server (and related game manager) accept a client's request to switch sides in a room
        ///         (from players to spectators or vice versa), but only if there isn't an ongoing match linked to this room.
        ///         (room/game manager ID must exist within the message object.)
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        ACCEPT_PLAYER_SWITCH_SIDES_ROOM,

        /// <summary>
        ///     Used when the server sends a text message to all clients in the lobby chat.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SERVER_BROADCAST_NEW_CHAT_MESSAGE_LOBBY,

        /// <summary>
        ///     Used when the server sends a text message to all clients in a specific room chat.
        ///         (room/game manager ID must exist within the message object.)
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SERVER_BROADCAST_NEW_CHAT_MESSAGE_ROOM,

        /// <summary>
        ///     Used whenever a new game (and a game manager) is created .
        ///     Every manager needs to be identified by using a unique ID.
        ///     This ID, along with all the important room/game info is sent back to the client that created the room.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        GAME_ROOM_CREATION_REQUEST_ACCEPTED,

        /// <summary>
        ///     Used to send back more information about a specific room to the client demanding it.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_GAME_ROOM_DESCRIPTION,

        /// <summary>
        ///     Used when a client requested to join an existing room and the server has established the link.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        GAME_ROOM_JOIN_REQUEST_ACCEPTED,

        /// <summary>
        ///     Used when a change occurs in the collection of rooms on the server
        ///         and the clients have to receive the updated data.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SERVER_BROADCAST_GAME_ROOM_COLLECTION,

        /// <summary>
        ///     Sent to the clients that are connected to a room where a match just began
        ///         and who need (or not) to initialize a controller for their snake locally,
        ///         depending on the data paired with this message type.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        RESPONSE_MATCH_STARTED,

        /// <summary>
        ///     Used whenever the arena matrix is changed in some way and the clients need to be aware of that change.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        SEND_ARENA_DATA,

        /// <summary>
        ///     Lets the user know that the server has accepted their request of leaving a room.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        RESPONSE_DISCONNECT_FROM_GAME_ROOM,

        /// <summary>
        ///     The server is ready to release all resources and the client's application can close.
        ///     <para/>
        ///     TECH NOTE: this will not use the regular flow of callback functions in the server's socket wrapper class.
        ///                Instead, it will use an asynchronous Send method, so when it is done, the server knows that
        ///                it can close the client's socket and release used resources.
        ///     <para/>
        ///     USAGE: server to client.
        /// </summary>
        RESPONSE_DISCONNECT_FROM_SERVER,

        /// <summary>
        ///     The server sends this message to every client connected to a specific game room
        ///         when the leader of the room left it and a new leader has been chosen.
        ///        <para />
        ///        USAGE: server to client.
        /// </summary>
        GAME_ROOM_LEADER_HAS_CHANGED,


        // ===================================================================================================


        // Test "commands".

        /// <summary>
        ///     Used to test if communication between two sockets works as expected.
        ///     <para/>
        ///     USAGE: client to server (and probably vice versa too).
        /// </summary>
        PING = 1000
    }
}