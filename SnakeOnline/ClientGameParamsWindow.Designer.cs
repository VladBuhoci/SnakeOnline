namespace SnakeOnline
{
    partial class ClientGameParamsWindow
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.passwordMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.roomNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.matchDurationBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.losePartsRadioButton = new System.Windows.Forms.RadioButton();
            this.nothingRadioButton = new System.Windows.Forms.RadioButton();
            this.growPartsRadioButton = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.maxSnakesAllowedBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.arenaHeightBox = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.arenaWidthBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.createRoomPanel = new System.Windows.Forms.Panel();
            this.createRoomButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchDurationBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSnakesAllowedBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arenaHeightBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arenaWidthBox)).BeginInit();
            this.createRoomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.passwordMaskedTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.roomNameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 103);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Room properties";
            // 
            // passwordMaskedTextBox
            // 
            this.passwordMaskedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordMaskedTextBox.Location = new System.Drawing.Point(89, 62);
            this.passwordMaskedTextBox.Name = "passwordMaskedTextBox";
            this.passwordMaskedTextBox.PasswordChar = '*';
            this.passwordMaskedTextBox.Size = new System.Drawing.Size(226, 20);
            this.passwordMaskedTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password";
            // 
            // roomNameTextBox
            // 
            this.roomNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roomNameTextBox.Location = new System.Drawing.Point(89, 25);
            this.roomNameTextBox.Name = "roomNameTextBox";
            this.roomNameTextBox.Size = new System.Drawing.Size(226, 20);
            this.roomNameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Room Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.matchDurationBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.losePartsRadioButton);
            this.groupBox2.Controls.Add(this.nothingRadioButton);
            this.groupBox2.Controls.Add(this.growPartsRadioButton);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.maxSnakesAllowedBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.arenaHeightBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.arenaWidthBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(13, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 217);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Game properties";
            // 
            // matchDurationBox
            // 
            this.matchDurationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.matchDurationBox.Location = new System.Drawing.Point(89, 177);
            this.matchDurationBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.matchDurationBox.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.matchDurationBox.Name = "matchDurationBox";
            this.matchDurationBox.Size = new System.Drawing.Size(64, 20);
            this.matchDurationBox.TabIndex = 11;
            this.matchDurationBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Duration";
            // 
            // losePartsRadioButton
            // 
            this.losePartsRadioButton.AutoSize = true;
            this.losePartsRadioButton.Location = new System.Drawing.Point(198, 145);
            this.losePartsRadioButton.Name = "losePartsRadioButton";
            this.losePartsRadioButton.Size = new System.Drawing.Size(107, 17);
            this.losePartsRadioButton.TabIndex = 9;
            this.losePartsRadioButton.TabStop = true;
            this.losePartsRadioButton.Text = "losing body parts.";
            this.losePartsRadioButton.UseVisualStyleBackColor = true;
            // 
            // nothingRadioButton
            // 
            this.nothingRadioButton.AutoSize = true;
            this.nothingRadioButton.Checked = true;
            this.nothingRadioButton.Location = new System.Drawing.Point(198, 121);
            this.nothingRadioButton.Name = "nothingRadioButton";
            this.nothingRadioButton.Size = new System.Drawing.Size(63, 17);
            this.nothingRadioButton.TabIndex = 8;
            this.nothingRadioButton.TabStop = true;
            this.nothingRadioButton.Text = "nothing.";
            this.nothingRadioButton.UseVisualStyleBackColor = true;
            // 
            // growPartsRadioButton
            // 
            this.growPartsRadioButton.AutoSize = true;
            this.growPartsRadioButton.Location = new System.Drawing.Point(198, 97);
            this.growPartsRadioButton.Name = "growPartsRadioButton";
            this.growPartsRadioButton.Size = new System.Drawing.Size(117, 17);
            this.growPartsRadioButton.TabIndex = 7;
            this.growPartsRadioButton.Text = "growing body parts.";
            this.growPartsRadioButton.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Eating the wrong food leads to:";
            // 
            // maxSnakesAllowedBox
            // 
            this.maxSnakesAllowedBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxSnakesAllowedBox.Location = new System.Drawing.Point(251, 62);
            this.maxSnakesAllowedBox.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.maxSnakesAllowedBox.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.maxSnakesAllowedBox.Name = "maxSnakesAllowedBox";
            this.maxSnakesAllowedBox.Size = new System.Drawing.Size(64, 20);
            this.maxSnakesAllowedBox.TabIndex = 5;
            this.maxSnakesAllowedBox.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(177, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Maximum number of snakes allowed";
            // 
            // arenaHeightBox
            // 
            this.arenaHeightBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arenaHeightBox.Location = new System.Drawing.Point(251, 27);
            this.arenaHeightBox.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.arenaHeightBox.Name = "arenaHeightBox";
            this.arenaHeightBox.Size = new System.Drawing.Size(64, 20);
            this.arenaHeightBox.TabIndex = 3;
            this.arenaHeightBox.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.arenaHeightBox.ValueChanged += new System.EventHandler(this.arenaHeightBox_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(178, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Arena Height";
            // 
            // arenaWidthBox
            // 
            this.arenaWidthBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arenaWidthBox.Location = new System.Drawing.Point(89, 27);
            this.arenaWidthBox.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.arenaWidthBox.Name = "arenaWidthBox";
            this.arenaWidthBox.Size = new System.Drawing.Size(64, 20);
            this.arenaWidthBox.TabIndex = 1;
            this.arenaWidthBox.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.arenaWidthBox.ValueChanged += new System.EventHandler(this.arenaWidthBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Arena Width";
            // 
            // createRoomPanel
            // 
            this.createRoomPanel.Controls.Add(this.createRoomButton);
            this.createRoomPanel.Location = new System.Drawing.Point(13, 336);
            this.createRoomPanel.Name = "createRoomPanel";
            this.createRoomPanel.Size = new System.Drawing.Size(339, 34);
            this.createRoomPanel.TabIndex = 2;
            // 
            // createRoomButton
            // 
            this.createRoomButton.AutoSize = true;
            this.createRoomButton.Location = new System.Drawing.Point(130, 5);
            this.createRoomButton.Name = "createRoomButton";
            this.createRoomButton.Size = new System.Drawing.Size(79, 23);
            this.createRoomButton.TabIndex = 0;
            this.createRoomButton.Text = "Create Room";
            this.createRoomButton.UseVisualStyleBackColor = true;
            this.createRoomButton.Click += new System.EventHandler(this.createRoomButton_Click);
            // 
            // ClientGameParamsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 382);
            this.Controls.Add(this.createRoomPanel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ClientGameParamsWindow";
            this.Text = "Game Room Info";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchDurationBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSnakesAllowedBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arenaHeightBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arenaWidthBox)).EndInit();
            this.createRoomPanel.ResumeLayout(false);
            this.createRoomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox roomNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox passwordMaskedTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown arenaWidthBox;
        private System.Windows.Forms.NumericUpDown arenaHeightBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown maxSnakesAllowedBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton growPartsRadioButton;
        private System.Windows.Forms.RadioButton nothingRadioButton;
        private System.Windows.Forms.RadioButton losePartsRadioButton;
        private System.Windows.Forms.NumericUpDown matchDurationBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel createRoomPanel;
        private System.Windows.Forms.Button createRoomButton;
    }
}