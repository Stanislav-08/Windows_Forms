using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class LoginPage : Form
    {
        private readonly AuthenticationService authService = new AuthenticationService();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            var token = await authService.Login(email, password);

            MessageBox.Show("TOKEN: " + (token ?? "NULL"));

            if (token != null)
            {
                MainPage mainPage = new MainPage();
                mainPage.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("LOGIN FAILED");
            }
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}