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
    public partial class ApplicationWindow : Form
    {
        private SnakeController snakeController;

        public ApplicationWindow()
        {
            InitializeComponent();

            SnakeGameManager.GetInstance().SetGameArenaPanel(gameArenaPanel);

            snakeController = new SnakeController(29, 30, Color.Red);
        }

        private void gameArenaPanel_Paint(object sender, PaintEventArgs e)
        {
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
            }
        }
        
        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
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

        private void ApplicationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop auxiliary game loop thread.
            SnakeGameManager.GetInstance().RequestAuxGameLoopThreadToEnd();
        }
    }
}
