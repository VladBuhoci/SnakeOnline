namespace SnakeOnline
{
    partial class ClientMenuWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.proceedToLobbyButton = new System.Windows.Forms.Button();
            this.mainMenuPanel = new System.Windows.Forms.Panel();
            this.connectToServerPanel = new System.Windows.Forms.Panel();
            this.connectToServerButton = new System.Windows.Forms.Button();
            this.serverAddressTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.connectWithUsernamePanel = new System.Windows.Forms.Panel();
            this.nicknameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenuPanel.SuspendLayout();
            this.connectToServerPanel.SuspendLayout();
            this.connectWithUsernamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // proceedToLobbyButton
            // 
            this.proceedToLobbyButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.proceedToLobbyButton.AutoSize = true;
            this.proceedToLobbyButton.Location = new System.Drawing.Point(138, 106);
            this.proceedToLobbyButton.Name = "proceedToLobbyButton";
            this.proceedToLobbyButton.Size = new System.Drawing.Size(105, 25);
            this.proceedToLobbyButton.TabIndex = 0;
            this.proceedToLobbyButton.Text = "Proceed";
            this.proceedToLobbyButton.UseVisualStyleBackColor = true;
            this.proceedToLobbyButton.Click += new System.EventHandler(this.proceedToLobbyButton_Click);
            // 
            // mainMenuPanel
            // 
            this.mainMenuPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainMenuPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainMenuPanel.Controls.Add(this.connectToServerPanel);
            this.mainMenuPanel.Controls.Add(this.connectWithUsernamePanel);
            this.mainMenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.mainMenuPanel.Name = "mainMenuPanel";
            this.mainMenuPanel.Size = new System.Drawing.Size(384, 411);
            this.mainMenuPanel.TabIndex = 1;
            // 
            // connectToServerPanel
            // 
            this.connectToServerPanel.Controls.Add(this.connectToServerButton);
            this.connectToServerPanel.Controls.Add(this.serverAddressTextBox);
            this.connectToServerPanel.Controls.Add(this.label2);
            this.connectToServerPanel.Location = new System.Drawing.Point(12, 13);
            this.connectToServerPanel.Name = "connectToServerPanel";
            this.connectToServerPanel.Size = new System.Drawing.Size(360, 190);
            this.connectToServerPanel.TabIndex = 3;
            // 
            // connectToServerButton
            // 
            this.connectToServerButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.connectToServerButton.AutoSize = true;
            this.connectToServerButton.Location = new System.Drawing.Point(138, 111);
            this.connectToServerButton.Name = "connectToServerButton";
            this.connectToServerButton.Size = new System.Drawing.Size(105, 25);
            this.connectToServerButton.TabIndex = 2;
            this.connectToServerButton.Text = "Connect to server";
            this.connectToServerButton.UseVisualStyleBackColor = true;
            this.connectToServerButton.Click += new System.EventHandler(this.connectToServerButton_Click);
            // 
            // serverAddressTextBox
            // 
            this.serverAddressTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverAddressTextBox.Location = new System.Drawing.Point(137, 54);
            this.serverAddressTextBox.Name = "serverAddressTextBox";
            this.serverAddressTextBox.Size = new System.Drawing.Size(105, 20);
            this.serverAddressTextBox.TabIndex = 4;
            this.serverAddressTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serverAddressTextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(51, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server address";
            // 
            // connectWithUsernamePanel
            // 
            this.connectWithUsernamePanel.Controls.Add(this.proceedToLobbyButton);
            this.connectWithUsernamePanel.Controls.Add(this.nicknameTextBox);
            this.connectWithUsernamePanel.Controls.Add(this.label1);
            this.connectWithUsernamePanel.Enabled = false;
            this.connectWithUsernamePanel.Location = new System.Drawing.Point(12, 209);
            this.connectWithUsernamePanel.Name = "connectWithUsernamePanel";
            this.connectWithUsernamePanel.Size = new System.Drawing.Size(360, 190);
            this.connectWithUsernamePanel.TabIndex = 2;
            // 
            // nicknameTextBox
            // 
            this.nicknameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nicknameTextBox.Location = new System.Drawing.Point(138, 59);
            this.nicknameTextBox.Name = "nicknameTextBox";
            this.nicknameTextBox.Size = new System.Drawing.Size(105, 20);
            this.nicknameTextBox.TabIndex = 1;
            this.nicknameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nicknameTextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(75, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nickname";
            // 
            // ClientMenuWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 411);
            this.Controls.Add(this.mainMenuPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.Name = "ClientMenuWindow";
            this.Text = "Snake Online - Menu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientMenuWindow_FormClosing);
            this.mainMenuPanel.ResumeLayout(false);
            this.connectToServerPanel.ResumeLayout(false);
            this.connectToServerPanel.PerformLayout();
            this.connectWithUsernamePanel.ResumeLayout(false);
            this.connectWithUsernamePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button proceedToLobbyButton;
        private System.Windows.Forms.Panel mainMenuPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nicknameTextBox;
        private System.Windows.Forms.Panel connectWithUsernamePanel;
        private System.Windows.Forms.Panel connectToServerPanel;
        private System.Windows.Forms.Button connectToServerButton;
        private System.Windows.Forms.TextBox serverAddressTextBox;
        private System.Windows.Forms.Label label2;
    }
}