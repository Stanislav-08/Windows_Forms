namespace App
{
    partial class MoviePage
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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            button1 = new Button();
            label6 = new Label();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            label7 = new Label();
            label8 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(25, 25);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(250, 350);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(321, 67);
            label1.Name = "label1";
            label1.Size = new Size(37, 20);
            label1.TabIndex = 1;
            label1.Text = "Year";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(424, 67);
            label2.Name = "label2";
            label2.Size = new Size(42, 20);
            label2.TabIndex = 2;
            label2.Text = "Time";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.Location = new Point(294, 25);
            label3.Name = "label3";
            label3.Size = new Size(202, 31);
            label3.TabIndex = 3;
            label3.Text = "TITLE";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(388, 67);
            label4.Name = "label4";
            label4.Size = new Size(18, 20);
            label4.TabIndex = 4;
            label4.Text = "●";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 421);
            label5.Name = "label5";
            label5.Size = new Size(37, 20);
            label5.TabIndex = 5;
            label5.Text = "Cast";
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImageLayout = ImageLayout.Center;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatStyle = FlatStyle.Popup;
            button1.ForeColor = Color.Transparent;
            button1.Location = new Point(33, 52);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Padding = new Padding(0, 0, 6, 7);
            button1.Size = new Size(322, 43);
            button1.TabIndex = 6;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(294, 226);
            label6.Name = "label6";
            label6.Size = new Size(31, 20);
            label6.TabIndex = 7;
            label6.Text = "bio";
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(25, 444);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(163, 230);
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Location = new Point(204, 444);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(163, 230);
            pictureBox3.TabIndex = 9;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(563, 445);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(163, 230);
            pictureBox4.TabIndex = 11;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Location = new Point(384, 445);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(163, 230);
            pictureBox5.TabIndex = 10;
            pictureBox5.TabStop = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(294, 178);
            label7.Name = "label7";
            label7.Size = new Size(47, 20);
            label7.TabIndex = 12;
            label7.Text = "genre";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(294, 125);
            label8.Name = "label8";
            label8.Size = new Size(56, 20);
            label8.TabIndex = 13;
            label8.Text = "creator";
            // 
            // panel1
            // 
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Location = new Point(617, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(388, 107);
            panel1.TabIndex = 14;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Location = new Point(617, 151);
            panel2.Name = "panel2";
            panel2.Size = new Size(388, 107);
            panel2.TabIndex = 15;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatStyle = FlatStyle.Popup;
            button2.ForeColor = Color.Transparent;
            button2.Location = new Point(19, 14);
            button2.Margin = new Padding(0);
            button2.Name = "button2";
            button2.Padding = new Padding(0, 0, 6, 7);
            button2.Size = new Size(100, 78);
            button2.TabIndex = 16;
            button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImageLayout = ImageLayout.Center;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatStyle = FlatStyle.Popup;
            button3.ForeColor = Color.Transparent;
            button3.Location = new Point(141, 14);
            button3.Margin = new Padding(0);
            button3.Name = "button3";
            button3.Padding = new Padding(0, 0, 6, 7);
            button3.Size = new Size(100, 78);
            button3.TabIndex = 17;
            button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.Transparent;
            button4.BackgroundImageLayout = ImageLayout.Center;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button4.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatStyle = FlatStyle.Popup;
            button4.ForeColor = Color.Transparent;
            button4.Location = new Point(264, 14);
            button4.Margin = new Padding(0);
            button4.Name = "button4";
            button4.Padding = new Padding(0, 0, 6, 7);
            button4.Size = new Size(100, 78);
            button4.TabIndex = 18;
            button4.UseVisualStyleBackColor = false;
            // 
            // MoviePage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1040, 999);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MoviePage";
            Text = "MoviePage";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button button1;
        private Label label6;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private Label label7;
        private Label label8;
        private Panel panel1;
        private Panel panel2;
        private Button button4;
        private Button button3;
        private Button button2;
    }
}