using SnakeOnlineServer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnlineServer
{
    public partial class MainWindow : Form
    {
        private GameServer server;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            mainMenuPanel.Hide();
            serverGeneralInfoPanel.Show();

            server = new GameServer(textBoxServerLog);
            server.SetUpServer();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ApplicationCleanUp()
        {
            // Clean everything.
            server.CleanUp();

            server = null;
        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            ApplicationCleanUp();

            serverGeneralInfoPanel.Hide();
            mainMenuPanel.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(this.Owner, "Are you sure you want to close this?", "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (server != null)
                {
                    ApplicationCleanUp();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
