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

        public ClientGameParamsWindow(GameClient clientSocket)
        {
            string publicIP = new System.Net.WebClient().DownloadString("https://api.ipify.org");

            InitializeComponent();

            socket = clientSocket;
            
            ipAddressTextBox.Text = publicIP;

            growPartsRadioButton.Text = FoodObject.EFFECT_GROW_PARTS;
            nothingRadioButton.Text = FoodObject.EFFECT_NOTHING;
            losePartsRadioButton.Text = FoodObject.EFFECT_LOSE_PARTS;
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

            SnakeGameDescriptor propertiesWrapper = new SnakeGameDescriptor
            {
                gameManagerID = -1,                                             // Temporary.
                roomName = roomNameTextBox.Text.Trim(),
                ipAddress = ipAddressTextBox.Text,
                roomPassword = passwordMaskedTextBox.Text,
                roomLeaderID = socket.GetUniquePlayerID(),
                

                arenaWidth = Decimal.ToInt32(arenaWidthBox.Value),
                arenaHeight = Decimal.ToInt32(arenaHeightBox.Value),
                maxSnakesAllowed = Decimal.ToInt32(maxSnakesAllowedBox.Value),
                foodEffect = GetChosenFoodEffect(),
                matchDuration = Decimal.ToInt32(matchDurationBox.Value)
            };

            socket.SendCreateGameRequestToServer(propertiesWrapper);

            Dispose();
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

        private string GetChosenFoodEffect()
        {
            if (growPartsRadioButton.Checked)
                return growPartsRadioButton.Text;
            else if (nothingRadioButton.Checked)
                return nothingRadioButton.Text;
            else if (losePartsRadioButton.Checked)
                return losePartsRadioButton.Text;

            throw new Exception("None of the three food effects were chosen.");
        }
    }
}