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
            closeButton = new Button();
            maximizeButton = new Button();
            minimizeButton = new Button();
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
            titleBarButtonsHolder.Controls.Add(closeButton);
            titleBarButtonsHolder.Controls.Add(maximizeButton);
            titleBarButtonsHolder.Controls.Add(minimizeButton);
            titleBarButtonsHolder.Dock = DockStyle.Right;
            titleBarButtonsHolder.Location = new Point(865, 0);
            titleBarButtonsHolder.Name = "titleBarButtonsHolder";
            titleBarButtonsHolder.Size = new Size(135, 32);
            titleBarButtonsHolder.TabIndex = 0;
            // 
            // closeButton
            // 
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Location = new Point(90, 0);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(45, 32);
            closeButton.TabIndex = 2;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += closeButton_Click;
            // 
            // maximizeButton
            // 
            maximizeButton.FlatAppearance.BorderSize = 0;
            maximizeButton.FlatStyle = FlatStyle.Flat;
            maximizeButton.Location = new Point(45, 0);
            maximizeButton.Name = "maximizeButton";
            maximizeButton.Size = new Size(45, 32);
            maximizeButton.TabIndex = 1;
            maximizeButton.UseVisualStyleBackColor = true;
            maximizeButton.Click += maximizeButton_Click;
            // 
            // minimizeButton
            // 
            minimizeButton.FlatAppearance.BorderSize = 0;
            minimizeButton.FlatStyle = FlatStyle.Flat;
            minimizeButton.Location = new Point(0, 0);
            minimizeButton.Name = "minimizeButton";
            minimizeButton.Size = new Size(45, 32);
            minimizeButton.TabIndex = 0;
            minimizeButton.UseVisualStyleBackColor = true;
            minimizeButton.Click += minimizeButton_Click;
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
        private Button minimizeButton;
        private Button closeButton;
        private Button maximizeButton;
    }
}
