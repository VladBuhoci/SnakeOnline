using System;
using System.Windows.Forms;

namespace SnakeOnline.Core
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
    }
}