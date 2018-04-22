using System;
using System.Windows.Forms;

namespace SnakeOnline.Core
{
    partial class ClientMenuWindow : Form
    {
        public ClientMenuWindow()
        {
            InitializeComponent();
        }

        private void connectToServerButton_Click(object sender, EventArgs e)
        {
            GameClient socket = new GameClient();
            ClientLobbyWindow clientLobbyWindow = new ClientLobbyWindow(socket);

            socket.clientLobbyWindow = clientLobbyWindow;
            socket.LoopConnect();
            
            // TODO: this line will go in the code of the Create Room button in the lobby.
            //client.SendCreateGameRequestToServer();

            this.Visible = false;
            clientLobbyWindow.ShowDialog(this);
        }
    }
}