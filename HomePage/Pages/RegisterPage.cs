using App;
using App.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace HomePage
{
    public partial class RegisterPage : Form
    {
        private readonly AuthenticationService AuthService;

        public RegisterPage()
        {
            InitializeComponent();

            AuthService=new AuthenticationService(new SupabaseClient());

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);
        }

        private void RegisterPage_Load(object sender, EventArgs e)
        {

        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string displayName = DisplayNameTextBox.Text;
            string dateOfBirth = DateOfBirthDateTimePicker.Value.ToString("yyyy-MM-dd");

            var success = await AuthService.Register(email, password, displayName, dateOfBirth);

            if (success)
            {
                MessageBox.Show("Registration successful!");
                MainPage mainPage = new MainPage();
                mainPage.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Registration failed.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}