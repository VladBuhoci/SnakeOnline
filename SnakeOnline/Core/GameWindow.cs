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
    public partial class GameWindow : Form
    {
        private SnakeController snakeController;

        public GameWindow()
        {
            InitializeComponent();

            SnakeGameManager.GetInstance().SetGameArenaPanel(gameArenaPanel);

            snakeController = new SnakeController(19, 30, Color.Red);

            SnakeGameManager.GetInstance().AddSnake(snakeController.GetControlledSnake());
            SnakeGameManager.GetInstance().SpawnFood(snakeController.GetControlledSnake().GetSnakeBodyParts().FirstOrDefault().color);
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

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop auxiliary game loop thread.
            SnakeGameManager.GetInstance().RequestAuxGameLoopThreadToEnd();
        }
    }
}
