using App.Pages;
using App.Services;
using System;
using System.Windows.Forms;

namespace App
{
    public partial class LoginPage : Form
    {
        private AuthenticationService AuthService;

        public LoginPage()
        {
            InitializeComponent();

            //Initialize AuthService
            AuthService = new AuthenticationService();

            //Title bar
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            //Secret password
            PasswordTextBox.UseSystemPasswordChar = true;

            button9.Image = Icons.Get("back_arrow");
        }

        //Login
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            //Empty field check
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            //Waiting login 
            var token = await AuthService.Login(email, password);
            if (token != null)
            {

                //Fetch and store username
                string displayName = await AuthService.GetDisplayName(token);
                Session.DisplayName = displayName;

                //Fetch and store token
                Session.AccessToken = token;

                //Admin check
                if (displayName == "Admin" && email == "admin@gmail.com")
                {
                    new AdminMainPage().Show();
                }
                else
                {
                    new MainPage().Show();
                }
                Hide();
            }
            else
            {
                MessageBox.Show("Login failed.");
            }
        }

        //Toggle password visibility
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }

        //Back button
        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new HomePage().Show();
        }
    }
}