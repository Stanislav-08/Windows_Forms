using App;
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
        private readonly HttpClient client = new HttpClient();

        private readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private readonly string apiKey = "sb_publishable_xFOf5KlGK5vMTPLz1cdH6Q_Q14a5n5f";

        public HomePage()
        {
            InitializeComponent();

            client.DefaultRequestHeaders.Add("apikey", apiKey);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            LoadMovies();
        }

        private async void LoadMovies()
        {
            var response = await client.GetAsync($"{supabaseUrl}/rest/v1/movies?select=*");
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var movies = JsonSerializer.Deserialize<List<Movie>>(json, options);

            listBox1.Items.Clear();

            if (movies == null) return;

            foreach (var m in movies)
            {
                listBox1.Items.Add($"{m.title} ({m.release_year}) - {m.rating} | {m.description}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void HomePage_Load(object sender, EventArgs e)
        {

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
    }

    public class Movie
    {
        public string title { get; set; }
        public int? release_year { get; set; }
        public float? rating { get; set; }
        public string description { get; set; }
    }
}