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
    partial class ClientMenuWindow : Form
    {
        private GameClient client;

        public ClientMenuWindow()
        {
            InitializeComponent();

            client = new GameClient(this);
        }

        private void connectToServerButton_Click(object sender, EventArgs e)
        {
            client.LoopConnect();
            client.SendCreateGameRequestToServer();
        }
    }
}
