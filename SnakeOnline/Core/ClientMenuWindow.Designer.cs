namespace SnakeOnline.Core
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
            this.connectToServerButton = new System.Windows.Forms.Button();
            this.mainMenuPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.nicknameTextBox = new System.Windows.Forms.TextBox();
            this.mainMenuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectToServerButton
            // 
            this.connectToServerButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.connectToServerButton.AutoSize = true;
            this.connectToServerButton.Location = new System.Drawing.Point(140, 243);
            this.connectToServerButton.Name = "connectToServerButton";
            this.connectToServerButton.Size = new System.Drawing.Size(105, 25);
            this.connectToServerButton.TabIndex = 0;
            this.connectToServerButton.Text = "Connect";
            this.connectToServerButton.UseVisualStyleBackColor = true;
            this.connectToServerButton.Click += new System.EventHandler(this.connectToServerButton_Click);
            // 
            // mainMenuPanel
            // 
            this.mainMenuPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainMenuPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainMenuPanel.Controls.Add(this.label1);
            this.mainMenuPanel.Controls.Add(this.nicknameTextBox);
            this.mainMenuPanel.Controls.Add(this.connectToServerButton);
            this.mainMenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainMenuPanel.Enabled = false;
            this.mainMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.mainMenuPanel.Name = "mainMenuPanel";
            this.mainMenuPanel.Size = new System.Drawing.Size(384, 411);
            this.mainMenuPanel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(77, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nickname";
            // 
            // nicknameTextBox
            // 
            this.nicknameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nicknameTextBox.Location = new System.Drawing.Point(140, 183);
            this.nicknameTextBox.Name = "nicknameTextBox";
            this.nicknameTextBox.Size = new System.Drawing.Size(105, 20);
            this.nicknameTextBox.TabIndex = 1;
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
            this.mainMenuPanel.ResumeLayout(false);
            this.mainMenuPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connectToServerButton;
        private System.Windows.Forms.Panel mainMenuPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nicknameTextBox;
    }
}