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
        public PersonPage()
        {
            InitializeComponent();
            button1.Image = Icons.Get("star");
            button2.Image = Icons.Get("outlined_thumb");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
    }

}
