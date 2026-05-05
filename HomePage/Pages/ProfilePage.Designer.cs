namespace App
{
    partial class ProfilePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfilePage));
            profilePictureBox = new PictureBox();
            usernameLabel = new Label();
            watchlistFlowLayoutPanel = new FlowLayoutPanel();
            panel1 = new Panel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)profilePictureBox).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // profilePictureBox
            // 
            profilePictureBox.Image = (Image)resources.GetObject("profilePictureBox.Image");
            profilePictureBox.Location = new Point(24, 24);
            profilePictureBox.Margin = new Padding(0);
            profilePictureBox.Name = "profilePictureBox";
            profilePictureBox.Size = new Size(160, 160);
            profilePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            profilePictureBox.TabIndex = 0;
            profilePictureBox.TabStop = false;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            usernameLabel.ForeColor = Color.White;
            usernameLabel.Location = new Point(208, 49);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new Size(179, 37);
            usernameLabel.TabIndex = 1;
            usernameLabel.Text = "Migel Iglesias";
            // 
            // watchlistFlowLayoutPanel
            // 
            watchlistFlowLayoutPanel.Location = new Point(24, 237);
            watchlistFlowLayoutPanel.Margin = new Padding(0);
            watchlistFlowLayoutPanel.Name = "watchlistFlowLayoutPanel";
            watchlistFlowLayoutPanel.Size = new Size(802, 212);
            watchlistFlowLayoutPanel.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(profilePictureBox);
            panel1.Controls.Add(usernameLabel);
            panel1.Controls.Add(watchlistFlowLayoutPanel);
            panel1.Location = new Point(150, 32);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(850, 468);
            panel1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 19.8000011F, FontStyle.Bold | FontStyle.Italic);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(24, 195);
            label1.Name = "label1";
            label1.Size = new Size(168, 32);
            label1.TabIndex = 3;
            label1.Text = "WATCHLIST";
            // 
            // ProfilePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(36, 38, 69);
            ClientSize = new Size(1000, 500);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ProfilePage";
            Text = "ProfilePage";
            ((System.ComponentModel.ISupportInitialize)profilePictureBox).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox profilePictureBox;
        private Label usernameLabel;
        private FlowLayoutPanel watchlistFlowLayoutPanel;
        private Panel panel1;
        private Label label1;
    }
}