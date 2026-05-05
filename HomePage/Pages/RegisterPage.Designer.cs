namespace App.Pages
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
            DateOfBirthDateTimePicker = new DateTimePicker();
            DateOfBirthLabel = new Label();
            label1 = new Label();
            button9 = new Button();
            SuspendLayout();
            // 
            // DisplayNameLabel
            // 
            DisplayNameLabel.AutoSize = true;
            DisplayNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            DisplayNameLabel.ForeColor = Color.White;
            DisplayNameLabel.Location = new Point(434, 123);
            DisplayNameLabel.Name = "DisplayNameLabel";
            DisplayNameLabel.Size = new Size(82, 15);
            DisplayNameLabel.TabIndex = 0;
            DisplayNameLabel.Text = "Display Name";
            // 
            // EmailLabel
            // 
            EmailLabel.AutoSize = true;
            EmailLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            EmailLabel.ForeColor = Color.White;
            EmailLabel.Location = new Point(457, 191);
            EmailLabel.Name = "EmailLabel";
            EmailLabel.Size = new Size(36, 15);
            EmailLabel.TabIndex = 1;
            EmailLabel.Text = "Email";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            PasswordLabel.ForeColor = Color.White;
            PasswordLabel.Location = new Point(448, 257);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(59, 15);
            PasswordLabel.TabIndex = 2;
            PasswordLabel.Text = "Password";
            // 
            // DisplayNameTextBox
            // 
            DisplayNameTextBox.Location = new Point(377, 141);
            DisplayNameTextBox.Name = "DisplayNameTextBox";
            DisplayNameTextBox.Size = new Size(200, 23);
            DisplayNameTextBox.TabIndex = 3;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(377, 275);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(200, 23);
            PasswordTextBox.TabIndex = 4;
            // 
            // EmailTextBox
            // 
            EmailTextBox.Location = new Point(377, 209);
            EmailTextBox.Name = "EmailTextBox";
            EmailTextBox.Size = new Size(200, 23);
            EmailTextBox.TabIndex = 5;
            // 
            // RegisterButton
            // 
            RegisterButton.BackColor = SystemColors.Window;
            RegisterButton.FlatAppearance.BorderSize = 0;
            RegisterButton.FlatStyle = FlatStyle.Flat;
            RegisterButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            RegisterButton.Location = new Point(332, 416);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(300, 36);
            RegisterButton.TabIndex = 6;
            RegisterButton.Text = "Register";
            RegisterButton.UseVisualStyleBackColor = false;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(599, 277);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(112, 19);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "Show Password";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // DateOfBirthDateTimePicker
            // 
            DateOfBirthDateTimePicker.Location = new Point(377, 337);
            DateOfBirthDateTimePicker.Name = "DateOfBirthDateTimePicker";
            DateOfBirthDateTimePicker.Size = new Size(200, 23);
            DateOfBirthDateTimePicker.TabIndex = 8;
            // 
            // DateOfBirthLabel
            // 
            DateOfBirthLabel.AutoSize = true;
            DateOfBirthLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            DateOfBirthLabel.ForeColor = Color.White;
            DateOfBirthLabel.Location = new Point(441, 319);
            DateOfBirthLabel.Name = "DateOfBirthLabel";
            DateOfBirthLabel.Size = new Size(79, 15);
            DateOfBirthLabel.TabIndex = 9;
            DateOfBirthLabel.Text = "Date of birth";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 19.8000011F, FontStyle.Bold | FontStyle.Italic);
            label1.ForeColor = Color.White;
            label1.Location = new Point(417, 48);
            label1.Name = "label1";
            label1.Size = new Size(121, 32);
            label1.TabIndex = 10;
            label1.Text = "Register";
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
            button9.TabIndex = 42;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // RegisterPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(36, 38, 69);
            ClientSize = new Size(1000, 500);
            Controls.Add(button9);
            Controls.Add(label1);
            Controls.Add(DateOfBirthLabel);
            Controls.Add(DateOfBirthDateTimePicker);
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
        private DateTimePicker DateOfBirthDateTimePicker;
        private Label DateOfBirthLabel;
        private Label label1;
        private Button button9;
    }
}