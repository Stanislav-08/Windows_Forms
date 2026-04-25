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

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private async void PersonPage_Load(object sender, EventArgs e)
        {
        }
    }

}
