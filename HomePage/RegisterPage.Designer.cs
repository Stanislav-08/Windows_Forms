namespace HomePage
{
    partial class RegisterPage
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
            DisplayNameLabel = new Label();
            EmailLabel = new Label();
            PasswordLabel = new Label();
            DisplayNameTextBox = new TextBox();
            PasswordTextBox = new TextBox();
            EmailTextBox = new TextBox();
            RegisterButton = new Button();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // DisplayNameLabel
            // 
            DisplayNameLabel.AutoSize = true;
            DisplayNameLabel.Location = new Point(419, 67);
            DisplayNameLabel.Name = "DisplayNameLabel";
            DisplayNameLabel.Size = new Size(80, 15);
            DisplayNameLabel.TabIndex = 0;
            DisplayNameLabel.Text = "Display Name";
            // 
            // EmailLabel
            // 
            EmailLabel.AutoSize = true;
            EmailLabel.Location = new Point(440, 145);
            EmailLabel.Name = "EmailLabel";
            EmailLabel.Size = new Size(36, 15);
            EmailLabel.TabIndex = 1;
            EmailLabel.Text = "Email";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new Point(431, 241);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(57, 15);
            PasswordLabel.TabIndex = 2;
            PasswordLabel.Text = "Password";
            // 
            // DisplayNameTextBox
            // 
            DisplayNameTextBox.Location = new Point(409, 85);
            DisplayNameTextBox.Name = "DisplayNameTextBox";
            DisplayNameTextBox.Size = new Size(100, 23);
            DisplayNameTextBox.TabIndex = 3;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(409, 259);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(100, 23);
            PasswordTextBox.TabIndex = 4;
            // 
            // EmailTextBox
            // 
            EmailTextBox.Location = new Point(409, 163);
            EmailTextBox.Name = "EmailTextBox";
            EmailTextBox.Size = new Size(100, 23);
            EmailTextBox.TabIndex = 5;
            // 
            // RegisterButton
            // 
            RegisterButton.Location = new Point(419, 336);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(75, 23);
            RegisterButton.TabIndex = 6;
            RegisterButton.Text = "Register";
            RegisterButton.UseVisualStyleBackColor = true;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(549, 262);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(83, 19);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // RegisterPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 500);
            Controls.Add(checkBox1);
            Controls.Add(RegisterButton);
            Controls.Add(EmailTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(DisplayNameTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(EmailLabel);
            Controls.Add(DisplayNameLabel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "RegisterPage";
            Text = "RegisterPage";
            Load += RegisterPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label DisplayNameLabel;
        private Label EmailLabel;
        private Label PasswordLabel;
        private TextBox DisplayNameTextBox;
        private TextBox PasswordTextBox;
        private TextBox EmailTextBox;
        private Button RegisterButton;
        private CheckBox checkBox1;
    }
}