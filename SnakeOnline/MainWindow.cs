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
    public partial class ApplicationWindow : Form
    {
        int angle = 10;

        public ApplicationWindow()
        {
            InitializeComponent();
        }

        private void gameArenaPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawArc(new Pen(Color.White, 2.0f), new Rectangle(10, 10, 200, 200), angle, angle++);

            Refresh();
        }
    }
}
