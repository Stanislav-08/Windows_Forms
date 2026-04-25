using App.Databases;
using App.Services;
using App.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class MainPage : Form
    {
        private const string SupabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        private static readonly HttpClient Http = AppHttpClient.Instance;
        private static readonly SupabaseClient Supabase = new SupabaseClient();
        private static readonly TMDB_Service Tmdb = new TMDB_Service(Supabase);

        public MainPage()
        {
            InitializeComponent();

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            NavigationBar navigationBar = new NavigationBar();
            navigationBar.Dock = DockStyle.Left;
            Controls.Add(navigationBar);

            ConfigureFlow(flowLayoutPanel1);
            ConfigureFlow(flowLayoutPanel2);
        }

        private static void ConfigureFlow(FlowLayoutPanel flow)
        {
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.WrapContents = false;
            flow.AutoScroll = true;
            flow.HorizontalScroll.Visible = false;
            flow.VerticalScroll.Visible = false;
        }

        // -------------------------------------------------------------------
        // Form load — sync only if Supabase is empty, then load UI
        // -------------------------------------------------------------------
        private async void MainPage_Load(object sender, EventArgs e)
        {
            bool moviesExist = await DatabaseHasRows("movies");

            if (!moviesExist)
                await Tmdb.SyncAll();

            await LoadMovies();
            await LoadPeople();
        }

        private static async Task<bool> DatabaseHasRows(string table)
        {
            try
            {
                var rows = await Supabase.GetAll<object>(table);
                return rows != null && rows.Count > 0;
            }
            catch
            {
                return false; // assume empty → trigger sync
            }
        }

        // -------------------------------------------------------------------
        // Load movies from Supabase and build UI cards
        // -------------------------------------------------------------------
        private async Task LoadMovies()
        {
            var movies = await Supabase.GetAll<Movie>("movies?select=*,movie_genres(genres(name))");

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var movie in movies)
            {
                string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{movie.poster_path}";
                var card = CreateCard(movie.title, imageUrl);
                card.Tag = movie;
                AttachClickRecursive(card, () => MovieCard_Click(card));
                flowLayoutPanel1.Controls.Add(card);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        // -------------------------------------------------------------------
        // Load people from Supabase and build UI cards
        // -------------------------------------------------------------------
        private async Task LoadPeople()
        {
            var people = await Supabase.GetAll<Person>("people");

            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel2.Controls.Clear();

            foreach (var person in people)
            {
                string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";
                var card = CreateCard(person.name, imageUrl);
                card.Tag = person;
                AttachClickRecursive(card, () => PersonCard_Click(card));
                flowLayoutPanel2.Controls.Add(card);
            }

            flowLayoutPanel2.ResumeLayout();
        }

        // -------------------------------------------------------------------
        // Build a reusable image + label card
        // -------------------------------------------------------------------
        private Panel CreateCard(string text, string imageUrl)
        {
            var card = new Panel
            {
                Width = 150,
                Height = 283,
                BackColor = Color.FromArgb(30, 30, 30),
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 5, 0)
            };

            var poster = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 225,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            _ = LoadImageAsync(poster, imageUrl);

            var label = new Label
            {
                Text = text,
                Dock = DockStyle.Bottom,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(label);
            card.Controls.Add(poster);

            return card;
        }

        private static void AttachClickRecursive(Control ctrl, Action onClick)
        {
            ctrl.Click += (s, e) => onClick();
            foreach (Control child in ctrl.Controls)
                AttachClickRecursive(child, onClick);
        }

        // Load image async — keep MemoryStream alive (Image.FromStream needs it open)
        private static async Task LoadImageAsync(PictureBox box, string url)
        {
            try
            {
                var bytes = await Http.GetByteArrayAsync(url);
                var ms = new MemoryStream(bytes); // intentionally not disposed
                box.Image = Image.FromStream(ms);
            }
            catch
            {
                if (!box.IsDisposed)
                    box.BackColor = Color.DarkGray;
            }
        }

        // -------------------------------------------------------------------
        // Navigation
        // -------------------------------------------------------------------
        private void MovieCard_Click(Panel card)
        {
            if (card?.Tag is not Movie movie) return;

            string posterPath = $"{SupabaseUrl}/storage/v1/object/public/pictures/{movie.poster_path}";
            var genreNames = movie.movie_genres?
                .Select(mg => mg.genres?.name)
                .Where(n => n != null)
                .ToList();
            string genres = string.Join(", ", genreNames ?? new List<string>());

            new MoviePage(movie.title, movie.duration_minutes, movie.rating, movie.release_year,
                          movie.description, posterPath, movie.status, movie.adult,
                          movie.director, genres).Show();
            Hide();
        }

        private void PersonCard_Click(Panel card)
        {
            if (card?.Tag is not Person person) return;

            string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";
            new PersonPage(person.name, person.info, imageUrl).Show();
            Hide();
        }
    }
}