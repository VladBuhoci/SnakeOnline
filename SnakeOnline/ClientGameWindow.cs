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
        private SnakeGameManagerCL snakeGameManagerCL;
        private GameClient gameClient;
        private Form clientMenuWindow;
        private SnakeController snakeController;

        public ClientGameWindow(GameClient client, Form clientMainMenuWindow, int uniqueGameManagerID)
        {
            InitializeComponent();
            
            snakeGameManagerCL = new SnakeGameManagerCL(gameArenaPane, uniqueGameManagerID);
            gameClient = client;
            gameClient.snakeGameManagerCL = snakeGameManagerCL;
            clientMenuWindow = clientMainMenuWindow;

            // TODO: should be created as a request from the server once the match has begun.
            //       Have some sort of method here that will be called by the client, in order to instantiate a controller.
            //snakeController = new SnakeController(client, 19, 30, Color.Red);

            //SnakeGameManager.GetInstance().SetGameArenaPane(gameArenaPane);
            
            //bApplicationAttemptsClosing = false;

            // Snake handling will be migrated on the server.
            //SnakeGameManager.GetInstance().AddSnake(snakeController.GetControlledSnake());

            // Food spawning will be migrated on the server.
            //SnakeGameManager.GetInstance().SpawnFood(snakeController.GetControlledSnake().GetSnakeBodyParts().FirstOrDefault().color);
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
        
        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            snakeGameManagerCL = null;
            snakeController = null;
        }
    }
}
