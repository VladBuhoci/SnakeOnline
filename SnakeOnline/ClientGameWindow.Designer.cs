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
            this.gameViewportPanel = new System.Windows.Forms.Panel();
            this.playersGroupBox = new System.Windows.Forms.GroupBox();
            this.playerListBox = new System.Windows.Forms.ListBox();
            this.spectatorsGroupBox = new System.Windows.Forms.GroupBox();
            this.spectatorListBox = new System.Windows.Forms.ListBox();
            this.roomMenuGroupBox = new System.Windows.Forms.GroupBox();
            this.roomLeaderLabel = new System.Windows.Forms.Label();
            this.switchSidesButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.roomSettingsButton = new System.Windows.Forms.Button();
            this.startMatchButton = new System.Windows.Forms.Button();
            this.roomChatGroupBox = new System.Windows.Forms.GroupBox();
            this.sendRoomChatMessageButton = new System.Windows.Forms.Button();
            this.roomChatTextToSendTextBox = new System.Windows.Forms.TextBox();
            this.roomChatTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gameArenaPane)).BeginInit();
            this.gameViewportPanel.SuspendLayout();
            this.playersGroupBox.SuspendLayout();
            this.spectatorsGroupBox.SuspendLayout();
            this.roomMenuGroupBox.SuspendLayout();
            this.roomChatGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameArenaPane
            // 
            this.gameArenaPane.BackColor = System.Drawing.Color.White;
            this.gameArenaPane.Location = new System.Drawing.Point(0, 0);
            this.gameArenaPane.Name = "gameArenaPane";
            this.gameArenaPane.Size = new System.Drawing.Size(500, 500);
            this.gameArenaPane.TabIndex = 1;
            this.gameArenaPane.TabStop = false;
            this.gameArenaPane.Paint += new System.Windows.Forms.PaintEventHandler(this.gameArenaPane_Paint);
            // 
            // gameViewportPanel
            // 
            this.gameViewportPanel.Controls.Add(this.gameArenaPane);
            this.gameViewportPanel.Location = new System.Drawing.Point(12, 12);
            this.gameViewportPanel.Name = "gameViewportPanel";
            this.gameViewportPanel.Size = new System.Drawing.Size(500, 500);
            this.gameViewportPanel.TabIndex = 2;
            // 
            // playersGroupBox
            // 
            this.playersGroupBox.Controls.Add(this.playerListBox);
            this.playersGroupBox.Location = new System.Drawing.Point(518, 12);
            this.playersGroupBox.Name = "playersGroupBox";
            this.playersGroupBox.Size = new System.Drawing.Size(170, 186);
            this.playersGroupBox.TabIndex = 3;
            this.playersGroupBox.TabStop = false;
            this.playersGroupBox.Text = "Players";
            // 
            // playerListBox
            // 
            this.playerListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.Location = new System.Drawing.Point(7, 20);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new System.Drawing.Size(157, 158);
            this.playerListBox.TabIndex = 0;
            // 
            // spectatorsGroupBox
            // 
            this.spectatorsGroupBox.Controls.Add(this.spectatorListBox);
            this.spectatorsGroupBox.Location = new System.Drawing.Point(702, 13);
            this.spectatorsGroupBox.Name = "spectatorsGroupBox";
            this.spectatorsGroupBox.Size = new System.Drawing.Size(170, 185);
            this.spectatorsGroupBox.TabIndex = 4;
            this.spectatorsGroupBox.TabStop = false;
            this.spectatorsGroupBox.Text = "Spectators";
            // 
            // spectatorListBox
            // 
            this.spectatorListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spectatorListBox.FormattingEnabled = true;
            this.spectatorListBox.Location = new System.Drawing.Point(6, 19);
            this.spectatorListBox.Name = "spectatorListBox";
            this.spectatorListBox.Size = new System.Drawing.Size(158, 158);
            this.spectatorListBox.TabIndex = 0;
            // 
            // roomMenuGroupBox
            // 
            this.roomMenuGroupBox.Controls.Add(this.roomLeaderLabel);
            this.roomMenuGroupBox.Controls.Add(this.switchSidesButton);
            this.roomMenuGroupBox.Controls.Add(this.disconnectButton);
            this.roomMenuGroupBox.Controls.Add(this.roomSettingsButton);
            this.roomMenuGroupBox.Controls.Add(this.startMatchButton);
            this.roomMenuGroupBox.Location = new System.Drawing.Point(518, 204);
            this.roomMenuGroupBox.Name = "roomMenuGroupBox";
            this.roomMenuGroupBox.Size = new System.Drawing.Size(354, 97);
            this.roomMenuGroupBox.TabIndex = 5;
            this.roomMenuGroupBox.TabStop = false;
            this.roomMenuGroupBox.Text = "Room Menu";
            // 
            // roomLeaderLabel
            // 
            this.roomLeaderLabel.AutoSize = true;
            this.roomLeaderLabel.Location = new System.Drawing.Point(142, 32);
            this.roomLeaderLabel.Name = "roomLeaderLabel";
            this.roomLeaderLabel.Size = new System.Drawing.Size(73, 13);
            this.roomLeaderLabel.TabIndex = 4;
            this.roomLeaderLabel.Text = "Room leader: ";
            this.roomLeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // switchSidesButton
            // 
            this.switchSidesButton.Location = new System.Drawing.Point(184, 68);
            this.switchSidesButton.Name = "switchSidesButton";
            this.switchSidesButton.Size = new System.Drawing.Size(75, 23);
            this.switchSidesButton.TabIndex = 2;
            this.switchSidesButton.Text = "Switch";
            this.switchSidesButton.UseVisualStyleBackColor = true;
            this.switchSidesButton.Click += new System.EventHandler(this.switchSidesButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(273, 68);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 3;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // roomSettingsButton
            // 
            this.roomSettingsButton.Location = new System.Drawing.Point(95, 68);
            this.roomSettingsButton.Name = "roomSettingsButton";
            this.roomSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.roomSettingsButton.TabIndex = 1;
            this.roomSettingsButton.Text = "Settings";
            this.roomSettingsButton.UseVisualStyleBackColor = true;
            this.roomSettingsButton.Click += new System.EventHandler(this.roomSettingsButton_Click);
            // 
            // startMatchButton
            // 
            this.startMatchButton.Location = new System.Drawing.Point(6, 68);
            this.startMatchButton.Name = "startMatchButton";
            this.startMatchButton.Size = new System.Drawing.Size(75, 23);
            this.startMatchButton.TabIndex = 0;
            this.startMatchButton.Text = "Start";
            this.startMatchButton.UseVisualStyleBackColor = true;
            this.startMatchButton.Click += new System.EventHandler(this.startMatchButton_Click);
            // 
            // roomChatGroupBox
            // 
            this.roomChatGroupBox.Controls.Add(this.sendRoomChatMessageButton);
            this.roomChatGroupBox.Controls.Add(this.roomChatTextToSendTextBox);
            this.roomChatGroupBox.Controls.Add(this.roomChatTextBox);
            this.roomChatGroupBox.Location = new System.Drawing.Point(518, 307);
            this.roomChatGroupBox.Name = "roomChatGroupBox";
            this.roomChatGroupBox.Size = new System.Drawing.Size(354, 205);
            this.roomChatGroupBox.TabIndex = 6;
            this.roomChatGroupBox.TabStop = false;
            this.roomChatGroupBox.Text = "Room Chat";
            // 
            // sendRoomChatMessageButton
            // 
            this.sendRoomChatMessageButton.Location = new System.Drawing.Point(272, 179);
            this.sendRoomChatMessageButton.Name = "sendRoomChatMessageButton";
            this.sendRoomChatMessageButton.Size = new System.Drawing.Size(75, 20);
            this.sendRoomChatMessageButton.TabIndex = 2;
            this.sendRoomChatMessageButton.Text = "Send";
            this.sendRoomChatMessageButton.UseVisualStyleBackColor = true;
            this.sendRoomChatMessageButton.Click += new System.EventHandler(this.sendRoomChatMessageButton_Click);
            // 
            // roomChatTextToSendTextBox
            // 
            this.roomChatTextToSendTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roomChatTextToSendTextBox.Location = new System.Drawing.Point(6, 179);
            this.roomChatTextToSendTextBox.Name = "roomChatTextToSendTextBox";
            this.roomChatTextToSendTextBox.Size = new System.Drawing.Size(260, 20);
            this.roomChatTextToSendTextBox.TabIndex = 1;
            this.roomChatTextToSendTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.roomChatTextToSendTextBox_KeyDown);
            // 
            // roomChatTextBox
            // 
            this.roomChatTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roomChatTextBox.Location = new System.Drawing.Point(6, 19);
            this.roomChatTextBox.Multiline = true;
            this.roomChatTextBox.Name = "roomChatTextBox";
            this.roomChatTextBox.ReadOnly = true;
            this.roomChatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.roomChatTextBox.Size = new System.Drawing.Size(341, 154);
            this.roomChatTextBox.TabIndex = 0;
            // 
            // ClientGameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 524);
            this.Controls.Add(this.roomChatGroupBox);
            this.Controls.Add(this.roomMenuGroupBox);
            this.Controls.Add(this.spectatorsGroupBox);
            this.Controls.Add(this.playersGroupBox);
            this.Controls.Add(this.gameViewportPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ClientGameWindow";
            this.Text = "Snake Online Game Room";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gameArenaPane)).EndInit();
            this.gameViewportPanel.ResumeLayout(false);
            this.playersGroupBox.ResumeLayout(false);
            this.spectatorsGroupBox.ResumeLayout(false);
            this.roomMenuGroupBox.ResumeLayout(false);
            this.roomMenuGroupBox.PerformLayout();
            this.roomChatGroupBox.ResumeLayout(false);
            this.roomChatGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.PictureBox gameArenaPane;
        private System.Windows.Forms.Panel gameViewportPanel;
        private System.Windows.Forms.GroupBox playersGroupBox;
        private System.Windows.Forms.GroupBox spectatorsGroupBox;
        private System.Windows.Forms.GroupBox roomMenuGroupBox;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button roomSettingsButton;
        private System.Windows.Forms.Button startMatchButton;
        private System.Windows.Forms.GroupBox roomChatGroupBox;
        private System.Windows.Forms.TextBox roomChatTextToSendTextBox;
        private System.Windows.Forms.TextBox roomChatTextBox;
        private System.Windows.Forms.Button sendRoomChatMessageButton;
        private System.Windows.Forms.ListBox playerListBox;
        private System.Windows.Forms.ListBox spectatorListBox;
        private System.Windows.Forms.Button switchSidesButton;
        private System.Windows.Forms.Label roomLeaderLabel;
    }
}

