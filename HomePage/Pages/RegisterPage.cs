using App.Services;
using System;
using System.Windows.Forms;

namespace App.Pages
{
    public partial class RegisterPage : Form
    {
        private readonly AuthenticationService AuthService;

        public RegisterPage()
        {
            InitializeComponent();
            AuthService = new AuthenticationService(new SupabaseClient());

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);
        }

        private void RegisterPage_Load(object sender, EventArgs e) { }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();
            string displayName = DisplayNameTextBox.Text.Trim();
            string dateOfBirth = DateOfBirthDateTimePicker.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(displayName))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Register returns bool, not a token
            bool success = await AuthService.Register(email, password, displayName, dateOfBirth);

            if (success)
            {
                // After registering, log in to get the access token
                string? token = await AuthService.Login(email, password);

                if (token != null)
                {
                    Session.AccessToken = token;
                    new MainPage().Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Registered successfully but login failed. Please log in manually.");
                    new LoginPage().Show();
                    Hide();
                }
            }
            else
            {
                MessageBox.Show("Registration failed. Email may already be in use.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}