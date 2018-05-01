using System;
using System.Windows.Forms;

namespace SnakeOnline
{
    partial class ClientMenuWindow : Form
    {
        private GameClient socket;

        public ClientMenuWindow()
        {
            InitializeComponent();

            socket = new GameClient(this);
            socket.LoopConnect();
        }

        public void ConnectionWasSuccessful()
        {
            mainMenuPanel.Enabled = true;
        }

        private void nicknameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // When pressing enter, simulate a "Connect" button click.

            if (e.KeyCode == Keys.Enter)
            {
                connectToServerButton_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void connectToServerButton_Click(object sender, EventArgs e)
        {
            if (nicknameTextBox.Text.Trim().Length > 0)
            {
                socket.SendJoinLobbyRequestToServer(nicknameTextBox.Text.Trim());
            }
            else
            {
                MessageBox.Show(this, "Please enter a name.", "No input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClientMenuWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socket != null)
            {
                socket.SendDisconnectFromServerRequest();
            }
        }
    }
}