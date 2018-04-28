using SnakeOnlineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeOnline.Core
{
    sealed class SnakeGameManagerCL
    {
        private bool bApplicationAttemptsClosing;
        private int uniqueGameManagerID;

        public SnakeGameArenaObject[,] gameArenaObjects;

        public SnakeGameManagerCL(PictureBox gameArenaPane, int uniqueGameManagerID)
        {
            this.bApplicationAttemptsClosing = false;
            this.uniqueGameManagerID = uniqueGameManagerID;

            Thread auxGameLoopThread = new Thread(() => RenderLoop(gameArenaPane));
            auxGameLoopThread.Start();
        }

        public int GetUniqueGameManagerID()
        {
            return uniqueGameManagerID;
        }

        private void RenderLoop(PictureBox gameArenaPane)
        {
            while (! bApplicationAttemptsClosing)
            {
                if (gameArenaPane != null && gameArenaPane.IsHandleCreated)
                {
                    gameArenaPane.BeginInvoke(new MethodInvoker(delegate { gameArenaPane.Refresh(); }));
                }

                Thread.Sleep(100);
            }
            
            Thread.CurrentThread.Abort();
        }

        public void RequestAuxGameLoopThreadToEnd()
        {
            bApplicationAttemptsClosing = true;
        }
    }
}