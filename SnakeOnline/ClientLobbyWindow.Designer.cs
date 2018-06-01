namespace SnakeOnline
{
    partial class ClientLobbyWindow
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
            this.lobbyPanel = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.joinRoomButton = new System.Windows.Forms.Button();
            this.createRoomButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lobbyChat_SendButton = new System.Windows.Forms.Button();
            this.lobbyChat_TextToSendBox = new System.Windows.Forms.TextBox();
            this.lobbyChat_ChatBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clientsList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.roomsDataGridView = new System.Windows.Forms.DataGridView();
            this.RoomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HasPassword = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Players = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spectators = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lobbyPanel.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roomsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // lobbyPanel
            // 
            this.lobbyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lobbyPanel.BackColor = System.Drawing.Color.White;
            this.lobbyPanel.Controls.Add(this.groupBox4);
            this.lobbyPanel.Controls.Add(this.groupBox3);
            this.lobbyPanel.Controls.Add(this.groupBox2);
            this.lobbyPanel.Controls.Add(this.groupBox1);
            this.lobbyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lobbyPanel.Location = new System.Drawing.Point(0, 0);
            this.lobbyPanel.Name = "lobbyPanel";
            this.lobbyPanel.Size = new System.Drawing.Size(784, 461);
            this.lobbyPanel.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.joinRoomButton);
            this.groupBox4.Controls.Add(this.createRoomButton);
            this.groupBox4.Controls.Add(this.disconnectButton);
            this.groupBox4.ForeColor = System.Drawing.Color.Black;
            this.groupBox4.Location = new System.Drawing.Point(572, 252);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 196);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Menu";
            // 
            // joinRoomButton
            // 
            this.joinRoomButton.Location = new System.Drawing.Point(58, 90);
            this.joinRoomButton.Name = "joinRoomButton";
            this.joinRoomButton.Size = new System.Drawing.Size(93, 23);
            this.joinRoomButton.TabIndex = 2;
            this.joinRoomButton.Text = "Join Room";
            this.joinRoomButton.UseVisualStyleBackColor = true;
            this.joinRoomButton.Click += new System.EventHandler(this.joinRoomButton_Click);
            // 
            // createRoomButton
            // 
            this.createRoomButton.Location = new System.Drawing.Point(58, 43);
            this.createRoomButton.Name = "createRoomButton";
            this.createRoomButton.Size = new System.Drawing.Size(93, 23);
            this.createRoomButton.TabIndex = 1;
            this.createRoomButton.Text = "Create Room";
            this.createRoomButton.UseVisualStyleBackColor = true;
            this.createRoomButton.Click += new System.EventHandler(this.createRoomButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(58, 142);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(93, 23);
            this.disconnectButton.TabIndex = 0;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.lobbyChat_SendButton);
            this.groupBox3.Controls.Add(this.lobbyChat_TextToSendBox);
            this.groupBox3.Controls.Add(this.lobbyChat_ChatBox);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(13, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(550, 197);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Lobby Chat";
            // 
            // lobbyChat_SendButton
            // 
            this.lobbyChat_SendButton.Location = new System.Drawing.Point(450, 171);
            this.lobbyChat_SendButton.Name = "lobbyChat_SendButton";
            this.lobbyChat_SendButton.Size = new System.Drawing.Size(94, 20);
            this.lobbyChat_SendButton.TabIndex = 2;
            this.lobbyChat_SendButton.Text = "Send";
            this.lobbyChat_SendButton.UseVisualStyleBackColor = true;
            this.lobbyChat_SendButton.Click += new System.EventHandler(this.lobbyChat_SendButton_Click);
            // 
            // lobbyChat_TextToSendBox
            // 
            this.lobbyChat_TextToSendBox.Location = new System.Drawing.Point(7, 171);
            this.lobbyChat_TextToSendBox.Name = "lobbyChat_TextToSendBox";
            this.lobbyChat_TextToSendBox.Size = new System.Drawing.Size(437, 20);
            this.lobbyChat_TextToSendBox.TabIndex = 1;
            this.lobbyChat_TextToSendBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lobbyChat_TextToSendBox_KeyDown);
            // 
            // lobbyChat_ChatBox
            // 
            this.lobbyChat_ChatBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lobbyChat_ChatBox.Location = new System.Drawing.Point(7, 20);
            this.lobbyChat_ChatBox.Multiline = true;
            this.lobbyChat_ChatBox.Name = "lobbyChat_ChatBox";
            this.lobbyChat_ChatBox.ReadOnly = true;
            this.lobbyChat_ChatBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lobbyChat_ChatBox.Size = new System.Drawing.Size(537, 145);
            this.lobbyChat_ChatBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.clientsList);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(572, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 233);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "People";
            // 
            // clientsList
            // 
            this.clientsList.BackColor = System.Drawing.Color.WhiteSmoke;
            this.clientsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientsList.ForeColor = System.Drawing.Color.DimGray;
            this.clientsList.FormattingEnabled = true;
            this.clientsList.Location = new System.Drawing.Point(3, 16);
            this.clientsList.Name = "clientsList";
            this.clientsList.Size = new System.Drawing.Size(194, 214);
            this.clientsList.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.roomsDataGridView);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 233);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rooms";
            // 
            // roomsDataGridView
            // 
            this.roomsDataGridView.AllowUserToAddRows = false;
            this.roomsDataGridView.AllowUserToDeleteRows = false;
            this.roomsDataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.roomsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.roomsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RoomName,
            this.HasPassword,
            this.Players,
            this.Spectators,
            this.State,
            this.Id});
            this.roomsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roomsDataGridView.GridColor = System.Drawing.Color.White;
            this.roomsDataGridView.Location = new System.Drawing.Point(3, 16);
            this.roomsDataGridView.MultiSelect = false;
            this.roomsDataGridView.Name = "roomsDataGridView";
            this.roomsDataGridView.ReadOnly = true;
            this.roomsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.roomsDataGridView.Size = new System.Drawing.Size(544, 214);
            this.roomsDataGridView.TabIndex = 0;
            // 
            // RoomName
            // 
            this.RoomName.HeaderText = "Room Name";
            this.RoomName.Name = "RoomName";
            this.RoomName.ReadOnly = true;
            // 
            // HasPassword
            // 
            this.HasPassword.HeaderText = "Has Password";
            this.HasPassword.Name = "HasPassword";
            this.HasPassword.ReadOnly = true;
            this.HasPassword.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.HasPassword.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Players
            // 
            this.Players.HeaderText = "Players";
            this.Players.Name = "Players";
            this.Players.ReadOnly = true;
            // 
            // Spectators
            // 
            this.Spectators.HeaderText = "Spectators";
            this.Spectators.Name = "Spectators";
            this.Spectators.ReadOnly = true;
            // 
            // State
            // 
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // ClientLobbyWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.lobbyPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ClientLobbyWindow";
            this.Text = "Snake Online - Lobby";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientLobbyWindow_FormClosing);
            this.lobbyPanel.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.roomsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel lobbyPanel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button joinRoomButton;
        private System.Windows.Forms.Button createRoomButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button lobbyChat_SendButton;
        private System.Windows.Forms.TextBox lobbyChat_TextToSendBox;
        private System.Windows.Forms.TextBox lobbyChat_ChatBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox clientsList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView roomsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoomName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HasPassword;
        private System.Windows.Forms.DataGridViewTextBoxColumn Players;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spectators;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    }
}