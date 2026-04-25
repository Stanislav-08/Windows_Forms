namespace App.UI_Elements
{
    partial class NavigationBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ProfileButton = new Button();
            HomeButton = new Button();
            LogOutButton = new Button();
            menuStrip1 = new MenuStrip();
            HomeLabel = new ToolStripMenuItem();
            ProfileLabel = new ToolStripMenuItem();
            LogOutLabel = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProfileButton
            // 
            ProfileButton.BackColor = Color.Transparent;
            ProfileButton.BackgroundImageLayout = ImageLayout.Center;
            ProfileButton.FlatAppearance.BorderSize = 0;
            ProfileButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            ProfileButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            ProfileButton.FlatStyle = FlatStyle.Flat;
            ProfileButton.ForeColor = Color.Transparent;
            ProfileButton.Location = new Point(0, 48);
            ProfileButton.Margin = new Padding(0);
            ProfileButton.Name = "ProfileButton";
            ProfileButton.Size = new Size(48, 48);
            ProfileButton.TabIndex = 18;
            ProfileButton.UseVisualStyleBackColor = false;
            // 
            // HomeButton
            // 
            HomeButton.BackColor = Color.Transparent;
            HomeButton.BackgroundImageLayout = ImageLayout.Center;
            HomeButton.FlatAppearance.BorderSize = 0;
            HomeButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            HomeButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            HomeButton.FlatStyle = FlatStyle.Flat;
            HomeButton.ForeColor = Color.Transparent;
            HomeButton.Location = new Point(0, 0);
            HomeButton.Margin = new Padding(0);
            HomeButton.Name = "HomeButton";
            HomeButton.Size = new Size(48, 48);
            HomeButton.TabIndex = 19;
            HomeButton.UseVisualStyleBackColor = false;
            // 
            // LogOutButton
            // 
            LogOutButton.BackColor = Color.Transparent;
            LogOutButton.BackgroundImageLayout = ImageLayout.Center;
            LogOutButton.FlatAppearance.BorderSize = 0;
            LogOutButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LogOutButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LogOutButton.FlatStyle = FlatStyle.Flat;
            LogOutButton.ForeColor = Color.Transparent;
            LogOutButton.Location = new Point(0, 452);
            LogOutButton.Margin = new Padding(0);
            LogOutButton.Name = "LogOutButton";
            LogOutButton.Size = new Size(48, 48);
            LogOutButton.TabIndex = 20;
            LogOutButton.UseVisualStyleBackColor = false;
            // 
            // menuStrip1
            // 
            menuStrip1.AutoSize = false;
            menuStrip1.Dock = DockStyle.Right;
            menuStrip1.GripMargin = new Padding(0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { HomeLabel, ProfileLabel, LogOutLabel });
            menuStrip1.Location = new Point(48, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(0);
            menuStrip1.Size = new Size(102, 500);
            menuStrip1.TabIndex = 21;
            menuStrip1.Text = "menuStrip1";
            // 
            // HomeLabel
            // 
            HomeLabel.AutoSize = false;
            HomeLabel.Name = "HomeLabel";
            HomeLabel.Padding = new Padding(0);
            HomeLabel.Size = new Size(102, 48);
            HomeLabel.Text = "Home";
            HomeLabel.Click += HomeLabel_Click;
            // 
            // ProfileLabel
            // 
            ProfileLabel.AutoSize = false;
            ProfileLabel.Name = "ProfileLabel";
            ProfileLabel.Padding = new Padding(0);
            ProfileLabel.Size = new Size(102, 48);
            ProfileLabel.Text = "Profile";
            ProfileLabel.Click += ProfileLabel_Click;
            // 
            // LogOutLabel
            // 
            LogOutLabel.Alignment = ToolStripItemAlignment.Right;
            LogOutLabel.AutoSize = false;
            LogOutLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LogOutLabel.ForeColor = Color.Red;
            LogOutLabel.Name = "LogOutLabel";
            LogOutLabel.Padding = new Padding(0);
            LogOutLabel.Size = new Size(102, 48);
            LogOutLabel.Text = "Log out";
            LogOutLabel.Click += LogOutLabel_Click;
            // 
            // NavigationBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(LogOutButton);
            Controls.Add(HomeButton);
            Controls.Add(ProfileButton);
            Controls.Add(menuStrip1);
            Name = "NavigationBar";
            Size = new Size(150, 500);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button ProfileButton;
        private Button HomeButton;
        private Button LogOutButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem HomeLabel;
        private ToolStripMenuItem ProfileLabel;
        private ToolStripMenuItem LogOutLabel;
    }
}
