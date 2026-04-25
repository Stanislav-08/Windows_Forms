using App.UI_Elements;
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
    public partial class ProfilePage : Form
    {
        public ProfilePage()
        {
            InitializeComponent();

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            NavigationBar navigationBar = new NavigationBar();
            navigationBar.Dock = DockStyle.Left;
            Controls.Add(navigationBar);
        }
    }
}
