using App.Services;
using System;
using System.Windows.Forms;

namespace App.Pages
{
    public partial class RegisterPage : Form
    {
        private AuthenticationService AuthService;

        public RegisterPage()
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

        //Registration
        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();
            string displayName = DisplayNameTextBox.Text.Trim();
            string dateOfBirth = DateOfBirthDateTimePicker.Value.ToString("yyyy-MM-dd");


            //Empty field check
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(displayName))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            //Waiting registration 
            bool success = await AuthService.Register(email, password, displayName, dateOfBirth);

            if (success)
            {
                // Waiting login after registration
                string token = await AuthService.Login(email, password);

                if (token != null)
                {

                    //Store username
                    Session.DisplayName = displayName;

                    //Fetch and store token
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

        //Toggle password visibility
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new HomePage().Show();
        }
    }
}