using App.Services;
using System;
using System.Windows.Forms;

namespace App
{
    public partial class LoginPage : Form
    {
        private readonly SupabaseClient Supabase = new SupabaseClient();
        private readonly AuthenticationService AuthService;

        public LoginPage()
        {
            InitializeComponent();

            AuthService = new AuthenticationService(Supabase);

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            PasswordTextBox.UseSystemPasswordChar = true;
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var token = await AuthService.Login(email, password);

            if (token != null)
            {
                Session.AccessToken = token;
                new MainPage().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Login failed.");
            }
        }

        private void LoginPage_Load(object sender, EventArgs e) { }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}