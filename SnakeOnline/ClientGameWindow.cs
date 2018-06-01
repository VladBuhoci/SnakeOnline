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
        //private ClientLobbyWindow clientLobbyWindow;
        private SnakeGameManagerCL snakeGameManagerCL;
        private SnakeController snakeController;
        private int gameRoomID;

        // Refresh rate of the room, in seconds.
        private static int ROOM_REFRESH_RATE = 3;

        public ClientGameWindow(GameClient _socket, /*ClientLobbyWindow lobbyWindow,*/ int uniqueGameManagerID)
        {
            InitializeComponent();

            //clientLobbyWindow = lobbyWindow;
            snakeGameManagerCL = new SnakeGameManagerCL(gameArenaPane, uniqueGameManagerID);
            gameRoomID = uniqueGameManagerID;
            socket = _socket;

            // TODO: should be created as a request from the server once the match has begun.
            //       Have some sort of method here that will be called by the client, in order to instantiate a controller.
            //snakeController = new SnakeController(client, 19, 30, Color.Red);

            //SnakeGameManager.GetInstance().SetGameArenaPane(gameArenaPane);

            //bApplicationAttemptsClosing = false;

            // Snake handling will be migrated on the server.
            //SnakeGameManager.GetInstance().AddSnake(snakeController.GetControlledSnake());

            // Food spawning will be migrated on the server.
            //SnakeGameManager.GetInstance().SpawnFood(snakeController.GetControlledSnake().GetSnakeBodyParts().FirstOrDefault().color);

            Thread refreshRoomThread = new Thread(() => RefreshRoomLoop(socket));
            refreshRoomThread.Start();
        }

        private void RefreshRoomLoop(GameClient socket)
        {
            while (true)
            {
                if (socket != null)
                {
                    socket.SendUpdatedRoomPlayerListRequest(gameRoomID);
                    socket.SendUpdatedRoomSpectatorListRequest(gameRoomID);
                }

                Thread.Sleep(ROOM_REFRESH_RATE * 1000);
            }
        }

        private void gameArenaPane_Paint(object sender, PaintEventArgs e)
        {
            //foreach (SnakeGameArenaObject arenaObj in snakeGameManagerCL.gameArenaObjects)
            foreach (SnakeGameArenaObject arenaObj in SnakeGameManager.GetInstance().gameArenaObjects)
            {
                if (arenaObj is SnakeBodyObject)
                {
                    SnakeBodyObject snakePart = (SnakeBodyObject) arenaObj;

                    if (snakePart.isHead)
                    {
                        e.Graphics.DrawEllipse(new Pen(snakePart.color), new Rectangle(snakePart.posX * 10, snakePart.posY * 10, 10, 10));
                        e.Graphics.FillEllipse(new SolidBrush(snakePart.color), new Rectangle(snakePart.posX * 10, snakePart.posY * 10, 10, 10));
                    }
                    else
                    {
                        e.Graphics.DrawEllipse(new Pen(snakePart.color), new Rectangle(snakePart.posX * 10 + 1, snakePart.posY * 10 + 1, 8, 8));
                    }
                }
                else if (arenaObj is FoodObject)
                {
                    // Draw a diamond (for now).
                    e.Graphics.DrawPolygon(new Pen(arenaObj.color), new Point[] { new Point(arenaObj.posX * 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10), new Point(arenaObj.posX * 10 + 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10 + 10) });
                    e.Graphics.FillPolygon(new SolidBrush(arenaObj.color), new Point[] { new Point(arenaObj.posX * 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10), new Point(arenaObj.posX * 10 + 10, arenaObj.posY * 10 + 5), new Point(arenaObj.posX * 10 + 5, arenaObj.posY * 10 + 10) });
                }
            }
        }
        
        private void gameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (snakeController != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        snakeController.ChangeDirectionUp();
                        break;

                    case Keys.D:
                        snakeController.ChangeDirectionRight();
                        break;

                    case Keys.S:
                        snakeController.ChangeDirectionDown();
                        break;

                    case Keys.A:
                        snakeController.ChangeDirectionLeft();
                        break;
                }
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

        public void RequestGameToEnd()
        {
            if (snakeGameManagerCL != null)
            {
                snakeGameManagerCL.RequestAuxGameLoopThreadToEnd();
            }
        }

        public SnakeGameManagerCL GetSnakeGameManagerCL()
        {
            return snakeGameManagerCL;
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