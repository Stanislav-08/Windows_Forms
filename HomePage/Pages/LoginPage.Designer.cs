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
            SuspendLayout();
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(360, 309);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(75, 23);
            LoginButton.TabIndex = 11;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // EmailTextBox
            // 
            EmailTextBox.Location = new Point(350, 136);
            EmailTextBox.Name = "EmailTextBox";
            EmailTextBox.Size = new Size(100, 23);
            EmailTextBox.TabIndex = 10;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(350, 232);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(100, 23);
            PasswordTextBox.TabIndex = 9;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(372, 214);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(57, 15);
            PasswordLabel.TabIndex = 8;
            PasswordLabel.Text = "Password";
            // 
            // EmailLabel
            // 
            EmailLabel.AutoSize = true;
            EmailLabel.Location = new Point(381, 118);
            EmailLabel.Name = "EmailLabel";
            EmailLabel.Size = new Size(36, 15);
            EmailLabel.TabIndex = 7;
            EmailLabel.Text = "Email";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(462, 235);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(83, 19);
            checkBox1.TabIndex = 12;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // LoginPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(checkBox1);
            Controls.Add(LoginButton);
            Controls.Add(EmailTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(EmailLabel);
            Name = "LoginPage";
            Text = "LoginPage";
            Load += LoginPage_Load;
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
    }
}