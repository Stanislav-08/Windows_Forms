using App.Services;
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
    public partial class PersonPage : Form
    {
        public PersonPage(string name, string info, string url)
        {
            InitializeComponent();

            //Title Bar 
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);

            pictureBox1.Load(url);
            label1.Text = name;
            label2.Text = info;
            toolStripMenuItem2.Image = Icons.Get("menu");
            toolStripMenuItem2.Dock=DockStyle.Left;

            button1.Image = Icons.Get("star");
            button2.Image = Icons.Get("outlined_thumb");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var state = (string)button2.Tag;

            if (state == "off")
            {
                button2.Image = Icons.Get("outlined_thumb");
                button2.Tag = "on";
            }
            else
            {
                button2.Image = Icons.Get("filled_thumb");
                button2.Tag = "off";
            }
        }

        private async void PersonPage_Load(object sender, EventArgs e)
        {
        }
    }

}
