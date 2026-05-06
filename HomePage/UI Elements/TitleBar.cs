using App.Services;

namespace App
{
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();

            //Icons initialization
            CloseButton.Image = Icons.Get("close", 16);
            MinimizeButton.Image = Icons.Get("minimize", 16);
            MaximizeButton.Image = Icons.Get("maximize", 16);
        }

        //----------Close function----------

        private void closeButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        //----------Minimize function----------

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.FindForm().WindowState = FormWindowState.Minimized;
        }

        //----------Maximize function----------

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

        //----------Hover function and other strange things----------

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
