﻿namespace SnakeOnlineServer
{
    partial class MainWindow
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///     Clean up any resources being used.
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
        ///     Required method for Designer support - do not modify
        ///         the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.mainMenuPanel = new System.Windows.Forms.Panel();
            this.serverGeneralInfoPanel = new System.Windows.Forms.Panel();
            this.buttonStopServer = new System.Windows.Forms.Button();
            this.textBoxServerLog = new System.Windows.Forms.TextBox();
            this.mainMenuPanel.SuspendLayout();
            this.serverGeneralInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.Location = new System.Drawing.Point(216, 184);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(75, 23);
            this.buttonStartServer.TabIndex = 0;
            this.buttonStartServer.Text = "Start server";
            this.buttonStartServer.UseVisualStyleBackColor = true;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(216, 271);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // mainMenuPanel
            // 
            this.mainMenuPanel.Controls.Add(this.buttonStartServer);
            this.mainMenuPanel.Controls.Add(this.buttonExit);
            this.mainMenuPanel.Location = new System.Drawing.Point(12, 12);
            this.mainMenuPanel.Name = "mainMenuPanel";
            this.mainMenuPanel.Size = new System.Drawing.Size(500, 500);
            this.mainMenuPanel.TabIndex = 2;
            // 
            // serverGeneralInfoPanel
            // 
            this.serverGeneralInfoPanel.Controls.Add(this.buttonStopServer);
            this.serverGeneralInfoPanel.Controls.Add(this.textBoxServerLog);
            this.serverGeneralInfoPanel.Location = new System.Drawing.Point(12, 12);
            this.serverGeneralInfoPanel.Name = "serverGeneralInfoPanel";
            this.serverGeneralInfoPanel.Size = new System.Drawing.Size(500, 500);
            this.serverGeneralInfoPanel.TabIndex = 2;
            this.serverGeneralInfoPanel.Visible = false;
            // 
            // buttonStopServer
            // 
            this.buttonStopServer.Location = new System.Drawing.Point(232, 470);
            this.buttonStopServer.Name = "buttonStopServer";
            this.buttonStopServer.Size = new System.Drawing.Size(75, 23);
            this.buttonStopServer.TabIndex = 1;
            this.buttonStopServer.Text = "Stop server";
            this.buttonStopServer.UseVisualStyleBackColor = true;
            this.buttonStopServer.Click += new System.EventHandler(this.buttonStopServer_Click);
            // 
            // textBoxServerLog
            // 
            this.textBoxServerLog.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxServerLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxServerLog.ForeColor = System.Drawing.SystemColors.Info;
            this.textBoxServerLog.Location = new System.Drawing.Point(17, 16);
            this.textBoxServerLog.Multiline = true;
            this.textBoxServerLog.Name = "textBoxServerLog";
            this.textBoxServerLog.ReadOnly = true;
            this.textBoxServerLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxServerLog.Size = new System.Drawing.Size(317, 447);
            this.textBoxServerLog.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 524);
            this.Controls.Add(this.serverGeneralInfoPanel);
            this.Controls.Add(this.mainMenuPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Snake Online Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.mainMenuPanel.ResumeLayout(false);
            this.serverGeneralInfoPanel.ResumeLayout(false);
            this.serverGeneralInfoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartServer;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Panel mainMenuPanel;
        private System.Windows.Forms.Panel serverGeneralInfoPanel;
        private System.Windows.Forms.Button buttonStopServer;
        private System.Windows.Forms.TextBox textBoxServerLog;
    }
}

