using SnakeOnlineCore;
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
    partial class ClientGameParamsWindow : Form
    {
        private GameClient socket;

        private int roomID;
        private string roomPassword;

        public ClientGameParamsWindow(GameClient clientSocket, SnakeGameDescriptor? roomDescr)
        {
            InitializeComponent();

            socket = clientSocket;
            
            if (roomDescr != null)
            {
                roomNameTextBox.Enabled = false;
                roomNameTextBox.Text = roomDescr.Value.roomName;

                // Clients will have to type this password in order to connect to the room.
                roomPassword = roomDescr.Value.roomPassword;

                if (roomPassword.Length == 0)
                {
                    passwordMaskedTextBox.Enabled = false;
                }

                //arenaWidthBox.Enabled = false;
                arenaWidthBox.Text = roomDescr.Value.arenaWidth.ToString();

                //arenaHeightBox.Enabled = false;
                arenaHeightBox.Text = roomDescr.Value.arenaHeight.ToString();

                //maxSnakesAllowedBox.Enabled = false;
                maxSnakesAllowedBox.Text = roomDescr.Value.maxSnakesAllowed.ToString();
                
                gameProps.Enabled = false;
                
                matchDurationBox.Enabled = false;
                matchDurationBox.Text = roomDescr.Value.matchDuration.ToString();

                createRoomPanel.Visible = false;
                joinRoomPanel.Visible = true;
            }
        }

        private void arenaWidthBox_ValueChanged(object sender, EventArgs e)
        {
            ClampAllowedSnakesAmount();
        }

        private void arenaHeightBox_ValueChanged(object sender, EventArgs e)
        {
            ClampAllowedSnakesAmount();
        }

        private void createRoomButton_Click(object sender, EventArgs e)
        {
            // Room name cannot be empty.
            if (String.IsNullOrEmpty(roomNameTextBox.Text.Trim()))
            {
                MessageBox.Show(this, "Please enter a room name.", "Empty name", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            SnakeGameDescriptor propertiesWrapper = new SnakeGameDescriptor()
            {
                gameManagerID = -1,                                             // Temporary.
                roomName = roomNameTextBox.Text.Trim(),
                roomPassword = passwordMaskedTextBox.Text.Trim(),
                roomLeaderID = socket.GetUniquePlayerID(),
                currentPlayerCount = 1,
                roomState = GameRoomState.WAITING,

                arenaWidth = Decimal.ToInt32(arenaWidthBox.Value),
                arenaHeight = Decimal.ToInt32(arenaHeightBox.Value),
                maxSnakesAllowed = Decimal.ToInt32(maxSnakesAllowedBox.Value),
                matchDuration = Decimal.ToInt32(matchDurationBox.Value)
            };

            socket.SendCreateGameRequestToServer(propertiesWrapper);

            Dispose();
        }

        private void joinRoomButton_Click(object sender, EventArgs e)
        {
            // Validate password.
            if (roomPassword.Length > 0 && ! String.Equals(roomPassword, passwordMaskedTextBox.Text.Trim()))
            {
                MessageBox.Show(this, "Wrong password!", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            else
            {
                socket.SendJoinGameRequestToServer(roomID);

                Dispose();
            }
        }

        private void ClampAllowedSnakesAmount()
        {
            // Formula for minimum value:
            //      min. snakes = round (sqrt (sqrt (map width * map height)) + 1.5)
            maxSnakesAllowedBox.Minimum = Math.Round((decimal) (Math.Sqrt(Math.Sqrt(Decimal.ToDouble(arenaWidthBox.Value * arenaHeightBox.Value))) + 1.5));

            // Formula for minimum value:
            //      max. snakes = round (sqrt (sqrt (map width * map height)) * 2 + 5)
            maxSnakesAllowedBox.Maximum = Math.Round((decimal) (Math.Sqrt(Math.Sqrt(Decimal.ToDouble(arenaWidthBox.Value * arenaHeightBox.Value))) * 2 + 5));
        }
    }
}