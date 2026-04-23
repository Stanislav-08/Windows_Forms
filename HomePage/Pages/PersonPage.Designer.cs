namespace App
{
    partial class PersonPage
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
            menuStrip1 = new MenuStrip();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            button1 = new Button();
            button2 = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.AutoSize = false;
            menuStrip1.Dock = DockStyle.Left;
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem2, toolStripMenuItem3 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(181, 500);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.AutoSize = false;
            toolStripMenuItem2.BackColor = Color.Transparent;
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(64, 64);
            toolStripMenuItem2.TextImageRelation = TextImageRelation.Overlay;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.AutoSize = false;
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(174, 40);
            toolStripMenuItem3.Text = "toolStripMenuItem3";
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImageLayout = ImageLayout.Center;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.Transparent;
            button1.Location = new Point(274, 278);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Padding = new Padding(0, 0, 5, 5);
            button1.Size = new Size(48, 48);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.Transparent;
            button2.Location = new Point(373, 289);
            button2.Margin = new Padding(0);
            button2.Name = "button2";
            button2.Padding = new Padding(0, 0, 5, 5);
            button2.Size = new Size(48, 48);
            button2.TabIndex = 3;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(367, 105);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 103);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(415, 240);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 7;
            label1.Text = "label1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(413, 343);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 8;
            label2.Text = "label2";
            // 
            // PersonPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1000, 500);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuStrip1;
            Name = "PersonPage";
            Text = "PersonPage";
            Load += PersonPage_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private Button button1;
        private Button button2;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
    }
}