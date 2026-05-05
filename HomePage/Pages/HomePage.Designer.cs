namespace App.Pages
{
    partial class HomePage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePage));
            RegisterButton = new Button();
            LoginButton = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // RegisterButton
            // 
            RegisterButton.BackColor = SystemColors.ButtonHighlight;
            RegisterButton.Font = new Font("Arial", 10.2F, FontStyle.Bold);
            RegisterButton.Location = new Point(372, 362);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(125, 23);
            RegisterButton.TabIndex = 2;
            RegisterButton.Text = "Register";
            RegisterButton.UseVisualStyleBackColor = false;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // LoginButton
            // 
            LoginButton.BackColor = SystemColors.ButtonHighlight;
            LoginButton.Font = new Font("Arial", 10.2F, FontStyle.Bold);
            LoginButton.Location = new Point(503, 362);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(127, 23);
            LoginButton.TabIndex = 3;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = false;
            LoginButton.Click += LoginButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(37, 54);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(912, 192);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(429, 270);
            label2.Name = "label2";
            label2.Size = new Size(135, 16);
            label2.TabIndex = 38;
            label2.Text = "Continue as guest";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(488, 336);
            label1.Name = "label1";
            label1.Size = new Size(24, 16);
            label1.TabIndex = 39;
            label1.Text = "Or";
            // 
            // HomePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(36, 38, 69);
            ClientSize = new Size(1000, 500);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(LoginButton);
            Controls.Add(RegisterButton);
            FormBorderStyle = FormBorderStyle.None;
            Name = "HomePage";
            Text = "HomePage";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Button RegisterButton;
        private Button LoginButton;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
    }
}
