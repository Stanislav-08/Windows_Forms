namespace App.UI_Elements
{
    partial class SearchBar
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
            SearchBarTextBox = new TextBox();
            SearchButton = new Button();
            SuspendLayout();
            // 
            // SearchBarTextBox
            // 
            SearchBarTextBox.BackColor = Color.White;
            SearchBarTextBox.BorderStyle = BorderStyle.None;
            SearchBarTextBox.Dock = DockStyle.Fill;
            SearchBarTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SearchBarTextBox.ForeColor = Color.Black;
            SearchBarTextBox.Location = new Point(0, 0);
            SearchBarTextBox.Margin = new Padding(0);
            SearchBarTextBox.MaximumSize = new Size(850, 24);
            SearchBarTextBox.MinimumSize = new Size(850, 24);
            SearchBarTextBox.Name = "SearchBarTextBox";
            SearchBarTextBox.PlaceholderText = " Search...";
            SearchBarTextBox.Size = new Size(850, 24);
            SearchBarTextBox.TabIndex = 1;
            // 
            // SearchButton
            // 
            SearchButton.BackColor = Color.FromArgb(230, 230, 230);
            SearchButton.Dock = DockStyle.Right;
            SearchButton.FlatAppearance.BorderSize = 0;
            SearchButton.FlatStyle = FlatStyle.Flat;
            SearchButton.Font = new Font("Arial", 10.2F, FontStyle.Bold);
            SearchButton.ForeColor = Color.Black;
            SearchButton.Location = new Point(850, 0);
            SearchButton.Margin = new Padding(0);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(150, 24);
            SearchButton.TabIndex = 0;
            SearchButton.Text = "Search";
            SearchButton.UseVisualStyleBackColor = false;
            // 
            // SearchBar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SearchButton);
            Controls.Add(SearchBarTextBox);
            Margin = new Padding(0);
            Name = "SearchBar";
            Size = new Size(1000, 24);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox SearchBarTextBox;
        private Button SearchButton;
    }
}
