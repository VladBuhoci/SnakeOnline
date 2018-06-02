using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline
{
    partial class ClientLobbyWindow : Form
    {
        private GameClient socket;

        // Refresh rate of the lobby, in seconds.
        private static int LOBBY_REFRESH_RATE = 3;

        public ClientLobbyWindow(GameClient socket)
        {
            InitializeComponent();
            
            this.socket = socket;
            this.socket.SendUpdatedLobbyPeopleListRequest();
            this.socket.SendUpdatedLobbyRoomListRequest();

            Thread refreshLobbyThread = new Thread(() => RefreshLobbyLoop(socket));
            refreshLobbyThread.Start();
        }

        private void RefreshLobbyLoop(GameClient socket)
        {
            while (true)
            {
                Thread.Sleep(LOBBY_REFRESH_RATE * 1000);

                if (socket != null)
                {
                    socket.SendUpdatedLobbyPeopleListRequest();
                    socket.SendUpdatedLobbyRoomListRequest();
                }

                break;
            }

            Thread.CurrentThread.Abort();
        }

        public void UpdateLobbyConnectedClientsList(string[] names)
        {
            clientsList.DataSource = names;
        }

        public void UpdateLobbyChat(string newMessage)
        {
            lobbyChat_ChatBox.AppendText(newMessage + "\n");
        }

        public void UpdateLobbyRoomList(SnakeGameShortDescriptor[] rooms)
        {
            // Clear the table.
            roomsDataGridView.Rows.Clear();

            // Populate the table.
            foreach (SnakeGameShortDescriptor roomDescr in rooms)
            {
                roomsDataGridView.Rows.Add(roomDescr.roomName, roomDescr.hasPassword, roomDescr.currentPlayerCount, roomDescr.currentSpectatorCount, roomDescr.roomState.ToString(), roomDescr.gameManagerID);
            }
        }

        private void lobbyChat_TextToSendBox_KeyDown(object sender, KeyEventArgs e)
        {
            // When pressing enter, simulate a "Send" button click.

            if (e.KeyCode == Keys.Enter)
            {
                lobbyChat_SendButton_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void lobbyChat_SendButton_Click(object sender, EventArgs e)
        {
            if (! String.IsNullOrEmpty(lobbyChat_TextToSendBox.Text))
            {
                socket.SendChatMessageInLobby(lobbyChat_TextToSendBox.Text.Trim());

                // Clear the text box after sending the message.
                lobbyChat_TextToSendBox.Clear();
            }
        }

        private void createRoomButton_Click(object sender, EventArgs e)
        {
            InstantiateGameParamsWindow(socket, null);
        }

        private void joinRoomButton_Click(object sender, EventArgs e)
        {
            int roomID = -1;

            if (roomsDataGridView.SelectedRows != null && roomsDataGridView.SelectedRows.Count > 0)
            {
                roomID = (int) roomsDataGridView.SelectedRows[0].Cells["Id"].Value;
            }

            if (roomID != -1)
            {
                socket.SendGameDescriptionRequestToServer(roomID);
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void InstantiateGameParamsWindow(GameClient _socket, SnakeGameDescriptor? descriptor)
        {
            ClientGameParamsWindow clientGameParamsWindow = new ClientGameParamsWindow(_socket, descriptor);

            clientGameParamsWindow.ShowDialog(this);
        }
        
        /// <summary>
        ///     Called when the window is being closed.
        /// </summary>
        private void ClientLobbyWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(this, "Are you sure you want to exit?", "Abandon", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (socket != null)
                {
                    socket.SendDisconnectFromServerRequest();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}