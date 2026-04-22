using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class MoviePage : Form
    {
        public MoviePage(string title, string description, string url)
        {
            InitializeComponent();

            //Title Bar 
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //tochka razdelqshta label 1-2-3
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
