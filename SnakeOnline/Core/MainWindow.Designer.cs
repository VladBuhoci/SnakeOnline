namespace SnakeOnline.Core
{
    partial class ApplicationWindow
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel gameArenaPanel;

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
            this.gameArenaPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // gameArenaPanel
            // 
            this.gameArenaPanel.BackColor = System.Drawing.Color.Black;
            this.gameArenaPanel.Location = new System.Drawing.Point(12, 12);
            this.gameArenaPanel.Name = "gameArenaPanel";
            this.gameArenaPanel.Size = new System.Drawing.Size(500, 500);
            this.gameArenaPanel.TabIndex = 0;
            this.gameArenaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.gameArenaPanel_Paint);
            // 
            // ApplicationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 524);
            this.Controls.Add(this.gameArenaPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ApplicationWindow";
            this.Text = "Snake Online";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainWindow_KeyDown);
            this.ResumeLayout(false);

        }


        #endregion


    }
}

