using App.Databases;
using App.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class MainPage : Form
    {
        private readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        static AuthenticationService db = new AuthenticationService();
        static HttpClient http = new HttpClient();

        public MainPage()
        {
            InitializeComponent();

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);

            ConfigureFlow(flowLayoutPanel1);
            ConfigureFlow(flowLayoutPanel2);
        }

        // Configure shared FlowLayoutPanel behavior
        private void ConfigureFlow(FlowLayoutPanel flow)
        {
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.WrapContents = false;
            flow.AutoScroll = true;
            flow.HorizontalScroll.Visible = false;
            flow.VerticalScroll.Visible = false;
        }

        private async void MainPage_Load(object sender, EventArgs e)
        {
            await LoadMovies();
            await LoadPeople();
        }

        // Load movies from database and build UI cards
        private async Task LoadMovies()
        {
            var movies = await db.GetDatabase<Movie>("movies") ?? new List<Movie>();

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var movie in movies)
            {
                string imageUrl =
                    $"{supabaseUrl}/storage/v1/object/public/pictures/{movie.poster_path}";

                var card = CreateCard(movie.title, imageUrl);

                card.Tag = movie;

                // click anywhere on card triggers navigation
                card.Click += (s, e) => MovieCard_Click(card);

                AttachClickRecursive(card, () => MovieCard_Click(card));

                flowLayoutPanel1.Controls.Add(card);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        // Load people from database and build UI cards
        private async Task LoadPeople()
        {
            var people = await db.GetDatabase<Person>("people") ?? new List<Person>();

            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel2.Controls.Clear();

            foreach (var person in people)
            {
                string imageUrl =
                    $"{supabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";

                var card = CreateCard(person.name, imageUrl);

                card.Tag = person;

                // click anywhere on card triggers navigation
                card.Click += (s, e) => PersonCard_Click(card);

                AttachClickRecursive(card, () => PersonCard_Click(card));

                flowLayoutPanel2.Controls.Add(card);
            }

            flowLayoutPanel2.ResumeLayout();
        }

        // Create reusable UI card (image + label)
        private Panel CreateCard(string text, string imageUrl)
        {
            Panel card = new Panel();
            card.Width = 150;
            card.Height = 283;
            card.BackColor = Color.FromArgb(30, 30, 30);
            card.Cursor = Cursors.Hand;
            card.Margin = new Padding(5, 0, 5, 0);

            PictureBox poster = new PictureBox();
            poster.Dock = DockStyle.Top;
            poster.Height = 225;
            poster.SizeMode = PictureBoxSizeMode.StretchImage;

            LoadImageAsync(poster, imageUrl);

            Label label = new Label();
            label.Text = text;
            label.Dock = DockStyle.Bottom;
            label.ForeColor = Color.White;
            label.TextAlign = ContentAlignment.MiddleCenter;

            card.Controls.Add(label);
            card.Controls.Add(poster);

            return card;
        }

        // Recursively attach click event to all child controls
        private void AttachClickRecursive(Control ctrl, Action onClick)
        {
            ctrl.Click += (s, e) => onClick();

            foreach (Control child in ctrl.Controls)
                AttachClickRecursive(child, onClick);
        }

        // Async image loading from URL
        private async void LoadImageAsync(PictureBox box, string url)
        {
            try
            {
                var bytes = await http.GetByteArrayAsync(url);

                using var ms = new MemoryStream(bytes);
                box.Image = Image.FromStream(ms);
            }
            catch
            {
                box.BackColor = Color.DarkGray;
            }
        }

        // Movie navigation handler
        private void MovieCard_Click(Panel card)
        {
            if (card?.Tag is not Movie movie) return;

            string imageUrl =
                $"{supabaseUrl}/storage/v1/object/public/pictures/{movie.poster_path}";

            MoviePage moviePage = new MoviePage(movie.title, movie.description, imageUrl);
            moviePage.ShowDialog();
            this.Hide();
        }

        // Person navigation handler
        private void PersonCard_Click(Panel card)
        {
            if (card?.Tag is not Person person) return;

            string imageUrl =
                $"{supabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";

            PersonPage personPage = new PersonPage(person.name, person.info, imageUrl);
            personPage.ShowDialog();
            this.Hide();
        }
    }
}