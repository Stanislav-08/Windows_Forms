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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace App
{
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
            closeButton.Image = Icons.Get("close_dark", 16);
            minimizeButton.Image = Icons.Get("minimize_dark", 16);
            maximizeButton.Image = Icons.Get("maximize_dark", 16);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.FindForm().WindowState = FormWindowState.Minimized;
        }
        private void maximizeButton_Click(object sender, EventArgs e)
        {
            if (this.FindForm().WindowState == FormWindowState.Normal)
            {
                this.FindForm().WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FindForm().WindowState = FormWindowState.Normal;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void titleBarHolder_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.FindForm().Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
