using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline.Core
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

        private void lobbyChat_SendButton_Click(object sender, EventArgs e)
        {
            if (! String.IsNullOrEmpty(lobbyChat_TextToSendBox.Text))
            {
                socket.SendChatMessageInLobby(lobbyChat_TextToSendBox.Text);

                // Clear the text box after sending the message.
                lobbyChat_TextToSendBox.Clear();
            }
        }

        private void createRoomButton_Click(object sender, EventArgs e)
        {

        }

        private void joinRoomButton_Click(object sender, EventArgs e)
        {

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     Called when the window is being closed.
        /// </summary>
        private void ClientLobbyWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socket != null)
            {
                socket.CleanUp();

                Application.Exit();
            }
        }
    }
}