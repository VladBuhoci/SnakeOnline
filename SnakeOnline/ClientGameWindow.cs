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
using SnakeOnlineCore;

namespace SnakeOnline
{
    partial class ClientGameWindow : Form
    {
        private GameClient socket;
        private SnakeController snakeController;
        private int gameRoomID;
        private GameRoomState gameRoomState;

        private SnakeGameArenaObject[, ] gameArenaObjects;

        public int GetGameRoomID()
        {
            return gameRoomID;
        }

        // Refresh rate of the room, in seconds.
        private static int ROOM_REFRESH_RATE = 1;

        public ClientGameWindow(GameClient _socket, string roomName, int uniqueGameManagerID, bool bIsThisLeader, string roomLeaderID, GameRoomState gameState, int arenaWidth, int arenaHeight)
        {
            InitializeComponent();

            // Set the title of the window.
            this.Text = String.Format("Snake Online Room - {0}", roomName);

            socket = _socket;
            gameRoomID = uniqueGameManagerID;
            gameRoomState = gameState;

            roomLeaderLabel.Text = String.Format("Room leader: {0}", roomLeaderID);

            // Scale and center the game preview window in its parent panel.
            gameArenaPane.Size = new Size(arenaWidth * 10, arenaHeight * 10);
            gameArenaPane.Location = new Point(gameViewportPanel.Width / 2 - arenaWidth * 5, gameViewportPanel.Height / 2 - arenaHeight * 5);

            if (! bIsThisLeader)
            {
                startMatchButton.Enabled = false;
                roomSettingsButton.Enabled = false;
            }
            
            Thread refreshRoomThread = new Thread(() => RefreshRoomLoop(socket));
            refreshRoomThread.Start();
        }

        private void RefreshRoomLoop(GameClient socket)
        {
            while (true)
            {
                Thread.Sleep(ROOM_REFRESH_RATE * 1000);

                if (socket != null)
                {
                    socket.SendUpdatedRoomPlayerListRequest(gameRoomID);
                    socket.SendUpdatedRoomSpectatorListRequest(gameRoomID);
                }

                break;
            }

            Thread.CurrentThread.Abort();
        }

        public void ChangeRoomLeader(string newLeaderID, bool bIsThisNewLeader)
        {
            roomLeaderLabel.Text = String.Format("Room leader: {0}", newLeaderID);

            if (bIsThisNewLeader)
            {
                startMatchButton.Enabled = true;
                roomSettingsButton.Enabled = true;
            }
            // else is redudant, since the client can never lose his/her leader rank, except when he/she disconnects.

            roomChatTextBox.AppendText(String.Format("[SYST]: New room leader is \"{0}\"", newLeaderID));
        }

        public void StartGameMatch(SnakeOrientation? initialOrientation)
        {
            gameRoomState = GameRoomState.PLAYING;
            gameArenaPane.BackColor = Color.Black;

            // Disable menu buttons.
            startMatchButton.Enabled = false;
            roomSettingsButton.Enabled = false;
            switchSidesButton.Enabled = false;

            if (initialOrientation != null)
            {
                MessageBox.Show(this, initialOrientation.ToString());
                snakeController = new SnakeController(socket, gameRoomID, initialOrientation.Value);
            }

            //gameArenaPane.Refresh();
        }

        public void UpdateArenaData(SnakeGameArenaObject[, ] gameArenaObjects)
        {
            this.gameArenaObjects = gameArenaObjects;

            gameArenaPane.Refresh();
        }

        public void EndGameMatch(bool bIsLeader)
        {
            gameRoomState = GameRoomState.WAITING;
            gameArenaObjects = null;
            snakeController = null;

            // Enable menu buttons.
            if (bIsLeader)
            {
                startMatchButton.Enabled = false;
                roomSettingsButton.Enabled = false;
            }
            switchSidesButton.Enabled = true;

            gameArenaPane.BackColor = Color.White;
        }

        private void gameArenaPane_Paint(object sender, PaintEventArgs e)
        {
            if (gameArenaObjects == null)
            {
                return;
            }

            foreach (SnakeGameArenaObject arenaObj in gameArenaObjects)
            {
                if (arenaObj is SnakeBodyObject)
                {
                    SnakeBodyObject snakePart = (SnakeBodyObject) arenaObj;

                    if (snakePart.isHead)
                    {
                        // Draw a bigger square.
                        e.Graphics.DrawEllipse(new Pen(snakePart.color), new Rectangle(snakePart.posX * 10, snakePart.posY * 10, 10, 10));
                        e.Graphics.FillEllipse(new SolidBrush(snakePart.color), new Rectangle(snakePart.posX * 10, snakePart.posY * 10, 10, 10));
                    }
                    else
                    {
                        // Draw a smaller square.
                        e.Graphics.DrawEllipse(new Pen(snakePart.color), new Rectangle(snakePart.posX * 10 + 1, snakePart.posY * 10 + 1, 8, 8));
                    }
                }
                else if (arenaObj is FoodObject)
                {
                    // Draw a diamond.
                    e.Graphics.DrawPolygon(new Pen(arenaObj.color), new Point[] { new Point(arenaObj.posX * 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10), new Point(arenaObj.posX * 10 + 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10 + 10) });
                    e.Graphics.FillPolygon(new SolidBrush(arenaObj.color), new Point[] { new Point(arenaObj.posX * 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10), new Point(arenaObj.posX * 10 + 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10 + 10) });
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (roomChatTextToSendTextBox.Focused)
            {
                // If the text box is selected, let this be passed forward by returning false.
                return false;
            }
            else
            {
                if (gameRoomState == GameRoomState.PLAYING)
                {
                    if (snakeController != null)
                    {
                        switch (keyData)
                        {
                            case Keys.W:
                                snakeController.ChangeDirectionUp();
                                return true;

                            case Keys.D:
                                snakeController.ChangeDirectionRight();
                                return true;

                            case Keys.S:
                                snakeController.ChangeDirectionDown();
                                return true;

                            case Keys.A:
                                snakeController.ChangeDirectionLeft();
                                return true;

                            case Keys.T:
                                roomChatTextToSendTextBox.Select();
                                return true;
                        }
                    }
                }

                // Call the base class.
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        
        public void UpdateRoomPlayerList(string[] playerNames)
        {
            playerListBox.DataSource = playerNames;
        }

        public void UpdateRoomSpectatorList(string[] spectatorNames)
        {
            spectatorListBox.DataSource = spectatorNames;
        }

        public void UpdateRoomChat(string newMessage)
        {
            roomChatTextBox.AppendText(newMessage + "\n");
        }

        private void startMatchButton_Click(object sender, EventArgs e)
        {
            if (playerListBox.DataSource is string[] playerList && playerList.Length >= 2)
            {
                socket.SendStartMatchRequestToServer(gameRoomID);
            }
        }

        private void roomSettingsButton_Click(object sender, EventArgs e)
        {

        }

        private void switchSidesButton_Click(object sender, EventArgs e)
        {
            socket.SendPlayerSwitchSidesInRoomRequest(gameRoomID);
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void roomChatTextToSendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // When pressing enter, simulate a "Send" button click.

            if (e.KeyCode == Keys.Enter)
            {
                sendRoomChatMessageButton_Click(sender, e);
                
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                gameArenaPane.Select();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void sendRoomChatMessageButton_Click(object sender, EventArgs e)
        {
            if (! String.IsNullOrEmpty(roomChatTextToSendTextBox.Text.Trim()))
            {
                socket.SendChatMessageInRoom(roomChatTextToSendTextBox.Text.Trim(), gameRoomID);

                // Clear the text box after sending the message.
                roomChatTextToSendTextBox.Clear();
            }
        }
        
        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(this, "Are you sure you want to leave?", "Abandon room", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (socket != null)
                {
                    socket.SendDisconnectFromGameRoomRequest(gameRoomID);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}