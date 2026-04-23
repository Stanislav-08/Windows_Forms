namespace HomePage
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
            textBox1 = new TextBox();
            button1 = new Button();
            RegisterButton = new Button();
            LoginButton = new Button();
            AppName = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(329, 214);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(532, 23);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(867, 214);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // RegisterButton
            // 
            RegisterButton.Location = new Point(243, 388);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(75, 23);
            RegisterButton.TabIndex = 2;
            RegisterButton.Text = "Register";
            RegisterButton.UseVisualStyleBackColor = true;
            RegisterButton.Click += RegisterButton_Click;
            // 
            // LoginButton
            // 
            LoginButton.Location = new Point(904, 388);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(75, 23);
            LoginButton.TabIndex = 3;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += LoginButton_Click;
            // 
            // AppName
            // 
            AppName.AutoSize = true;
            AppName.Font = new Font("Segoe UI", 40F);
            AppName.Location = new Point(516, 84);
            AppName.Name = "AppName";
            AppName.Size = new Size(229, 72);
            AppName.TabIndex = 4;
            AppName.Text = "CinePaw";
            // 
            // HomePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 500);
            Controls.Add(AppName);
            Controls.Add(LoginButton);
            Controls.Add(RegisterButton);
            Controls.Add(button1);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "HomePage";
            Text = "HomePage";
            Load += HomePage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private Button RegisterButton;
        private Button LoginButton;
        private Label AppName;
    }
}
