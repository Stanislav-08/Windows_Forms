using App;
using App.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Forms;

namespace HomePage
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
            ///ULETO BESHE TUK
            ///siskooo
            //Title Bar 
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            Hide();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.Show();
            Hide();
        }

        private async void HomePage_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ///
        }
    }

    public class Movie
    {
        public string title { get; set; }
        public int? release_year { get; set; }
        public float? rating { get; set; }
        public string description { get; set; }
    }
}