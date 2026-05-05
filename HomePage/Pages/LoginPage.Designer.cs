namespace App
{
    partial class LoginPage
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
            LoginButton = new Button();
            EmailTextBox = new TextBox();
            PasswordTextBox = new TextBox();
            PasswordLabel = new Label();
            EmailLabel = new Label();
            checkBox1 = new CheckBox();
            label1 = new Label();
            button9 = new Button();
            SuspendLayout();
            // 
            // LoginButton
            // 
            LoginButton.BackColor = SystemColors.Window;
            LoginButton.FlatAppearance.BorderSize = 0;
            LoginButton.FlatStyle = FlatStyle.Flat;
            LoginButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            LoginButton.Location = new Point(335, 303);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(300, 36);
            LoginButton.TabIndex = 11;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = false;
            LoginButton.Click += LoginButton_Click;
            // 
            // EmailTextBox
            // 
            EmailTextBox.Location = new Point(390, 181);
            EmailTextBox.Name = "EmailTextBox";
            EmailTextBox.Size = new Size(200, 23);
            EmailTextBox.TabIndex = 10;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(390, 245);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(200, 23);
            PasswordTextBox.TabIndex = 9;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            PasswordLabel.ForeColor = Color.White;
            PasswordLabel.Location = new Point(460, 227);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(59, 15);
            PasswordLabel.TabIndex = 8;
            PasswordLabel.Text = "Password";
            // 
            // EmailLabel
            // 
            EmailLabel.AutoSize = true;
            EmailLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            EmailLabel.ForeColor = Color.White;
            EmailLabel.Location = new Point(471, 163);
            EmailLabel.Name = "EmailLabel";
            EmailLabel.Size = new Size(36, 15);
            EmailLabel.TabIndex = 7;
            EmailLabel.Text = "Email";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(611, 247);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(112, 19);
            checkBox1.TabIndex = 12;
            checkBox1.Text = "Show Password";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 19.8000011F, FontStyle.Bold | FontStyle.Italic);
            label1.ForeColor = Color.White;
            label1.Location = new Point(448, 90);
            label1.Name = "label1";
            label1.Size = new Size(85, 32);
            label1.TabIndex = 13;
            label1.Text = "Login";
            // 
            // button9
            // 
            button9.BackColor = Color.Transparent;
            button9.BackgroundImageLayout = ImageLayout.Center;
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button9.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button9.FlatStyle = FlatStyle.Flat;
            button9.ForeColor = Color.Transparent;
            button9.Location = new Point(12, 48);
            button9.Margin = new Padding(0);
            button9.Name = "button9";
            button9.Size = new Size(48, 48);
            button9.TabIndex = 41;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // LoginPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(36, 38, 69);
            ClientSize = new Size(1000, 500);
            Controls.Add(button9);
            Controls.Add(label1);
            Controls.Add(checkBox1);
            Controls.Add(LoginButton);
            Controls.Add(EmailTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(EmailLabel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginPage";
            Text = "LoginPage";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoginButton;
        private TextBox EmailTextBox;
        private TextBox PasswordTextBox;
        private Label PasswordLabel;
        private Label EmailLabel;
        private CheckBox checkBox1;
        private Label label1;
        private Button button9;
    }
}