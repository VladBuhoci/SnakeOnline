using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline
{
    partial class ClientLobbyWindow : Form
    {
        private GameClient socket;

        public ClientLobbyWindow(GameClient socket)
        {
            InitializeComponent();

            this.socket = socket;
            this.socket.SendUpdatedLobbyPeopleListRequest();
        }

        public void UpdateConnectedClientsList(string[] names)
        {
            clientsList.DataSource = names;
        }

        public void UpdateLobbyChat(string newMessage)
        {
            lobbyChat_ChatBox.AppendText(newMessage + "\n");
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
            ClientGameParamsWindow clientGameParamsWindow = new ClientGameParamsWindow(socket);

            clientGameParamsWindow.ShowDialog(this);
        }

        private void joinRoomButton_Click(object sender, EventArgs e)
        {

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            this.Close();
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