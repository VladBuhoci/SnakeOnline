namespace SnakeOnline
{
    partial class ClientGameWindow
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// 
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gameArenaPane = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gameArenaPane)).BeginInit();
            this.SuspendLayout();
            // 
            // gameArenaPane
            // 
            this.gameArenaPane.BackColor = System.Drawing.Color.Black;
            this.gameArenaPane.Location = new System.Drawing.Point(12, 12);
            this.gameArenaPane.Name = "gameArenaPane";
            this.gameArenaPane.Size = new System.Drawing.Size(500, 500);
            this.gameArenaPane.TabIndex = 1;
            this.gameArenaPane.TabStop = false;
            this.gameArenaPane.Paint += new System.Windows.Forms.PaintEventHandler(this.gameArenaPane_Paint);
            // 
            // ClientGameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 524);
            this.Controls.Add(this.gameArenaPane);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ClientGameWindow";
            this.Text = "Snake Online";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameWindow_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gameWindow_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gameArenaPane)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.PictureBox gameArenaPane;
    }
}

