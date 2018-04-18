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
            this.SuspendLayout();
            // 
            // connectToServerButton
            // 
            this.connectToServerButton.Location = new System.Drawing.Point(187, 186);
            this.connectToServerButton.Name = "connectToServerButton";
            this.connectToServerButton.Size = new System.Drawing.Size(75, 35);
            this.connectToServerButton.TabIndex = 0;
            this.connectToServerButton.Text = "Connect to server";
            this.connectToServerButton.UseVisualStyleBackColor = true;
            this.connectToServerButton.Click += new System.EventHandler(this.connectToServerButton_Click);
            // 
            // ClientMenuWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 411);
            this.Controls.Add(this.connectToServerButton);
            this.Name = "ClientMenuWindow";
            this.Text = "ClientMenuWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connectToServerButton;
    }
}