using App.Services;
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
        private readonly AuthenticationService AuthService;

        public LoginPage()
        {
            InitializeComponent();
            AuthService = new AuthenticationService(new SupabaseClient());
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();
            var token = await AuthService.Login(email, password);

            //MessageBox.Show("TOKEN: " + (token ?? "NULL"));

            if (token != null)
            {
                MessageBox.Show("Login successful!");

                //Admin check
                if (email=="admin@gmail.com")
                {
                    AdminMainPage adminMainPage = new AdminMainPage();
                    adminMainPage.Show();
                }
                else
                {
                    MainPage mainPage = new MainPage();
                    mainPage.Show();
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login failed.");
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