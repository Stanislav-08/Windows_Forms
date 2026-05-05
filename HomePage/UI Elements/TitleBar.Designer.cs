namespace App
{
    partial class TitleBar
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
            titleBarHolder = new Panel();
            titleBarButtonsHolder = new Panel();
            CloseButton = new Button();
            MaximizeButton = new Button();
            MinimizeButton = new Button();
            titleBarHolder.SuspendLayout();
            titleBarButtonsHolder.SuspendLayout();
            SuspendLayout();
            // 
            // titleBarHolder
            // 
            titleBarHolder.Controls.Add(titleBarButtonsHolder);
            titleBarHolder.Dock = DockStyle.Top;
            titleBarHolder.Location = new Point(0, 0);
            titleBarHolder.Name = "titleBarHolder";
            titleBarHolder.Size = new Size(1000, 32);
            titleBarHolder.TabIndex = 0;
            titleBarHolder.MouseDown += titleBarHolder_MouseDown;
            // 
            // titleBarButtonsHolder
            // 
            titleBarButtonsHolder.Controls.Add(CloseButton);
            titleBarButtonsHolder.Controls.Add(MaximizeButton);
            titleBarButtonsHolder.Controls.Add(MinimizeButton);
            titleBarButtonsHolder.Dock = DockStyle.Right;
            titleBarButtonsHolder.Location = new Point(865, 0);
            titleBarButtonsHolder.Name = "titleBarButtonsHolder";
            titleBarButtonsHolder.Size = new Size(135, 32);
            titleBarButtonsHolder.TabIndex = 0;
            // 
            // CloseButton
            // 
            CloseButton.FlatAppearance.BorderSize = 0;
            CloseButton.FlatStyle = FlatStyle.Flat;
            CloseButton.Location = new Point(90, 0);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(45, 32);
            CloseButton.TabIndex = 2;
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += closeButton_Click;
            // 
            // MaximizeButton
            // 
            MaximizeButton.FlatAppearance.BorderSize = 0;
            MaximizeButton.FlatStyle = FlatStyle.Flat;
            MaximizeButton.Location = new Point(45, 0);
            MaximizeButton.Name = "MaximizeButton";
            MaximizeButton.Size = new Size(45, 32);
            MaximizeButton.TabIndex = 1;
            MaximizeButton.UseVisualStyleBackColor = true;
            MaximizeButton.Click += maximizeButton_Click;
            // 
            // MinimizeButton
            // 
            MinimizeButton.FlatAppearance.BorderSize = 0;
            MinimizeButton.FlatStyle = FlatStyle.Flat;
            MinimizeButton.Location = new Point(0, 0);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new Size(45, 32);
            MinimizeButton.TabIndex = 0;
            MinimizeButton.UseVisualStyleBackColor = true;
            MinimizeButton.Click += minimizeButton_Click;
            // 
            // TitleBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 25, 46);
            Controls.Add(titleBarHolder);
            Name = "TitleBar";
            Size = new Size(1000, 32);
            titleBarHolder.ResumeLayout(false);
            titleBarButtonsHolder.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel titleBarHolder;
        private Panel titleBarButtonsHolder;
        private Button MinimizeButton;
        private Button CloseButton;
        private Button MaximizeButton;
    }
}
