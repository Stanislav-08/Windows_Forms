using App.Databases;
using App.Services;
using App.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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

            NavigationBar navigationBar = new NavigationBar("main");
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
        // Form load
        // -------------------------------------------------------------------
        private async void MainPage_Load(object sender, EventArgs e)
        {
            try
            {
                bool moviesExist = await DatabaseHasRows("movies");
                bool peopleExist = await DatabaseHasRows("people");

                if (!moviesExist || !peopleExist)
                    await Tmdb.SyncAll();

                await LoadMovies();
                await LoadPeople();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MainPage_Load ERROR: {ex.Message}\n\n{ex.StackTrace}");
            }
        }

        private static async Task<bool> DatabaseHasRows(string table)
        {
            try
            {
                var rows = await Supabase.GetAll<JsonElement>($"{table}?select=id&limit=1");
                return rows != null && rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        // -------------------------------------------------------------------
        // Load movies — include genres and cast
        // -------------------------------------------------------------------
        private async Task LoadMovies()
        {
            try
            {
                var movies = await Supabase.GetAll<Movies>(
                    "movies?select=*,movie_genres(genres(name)),movie_people(role,people(name,profile_path))"
                );

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
            catch (Exception ex)
            {
                MessageBox.Show($"LoadMovies ERROR: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // Load people — include their movies
        // -------------------------------------------------------------------
        private async Task LoadPeople()
        {
            try
            {
                var people = await Supabase.GetAll<Person>(
                    "people?select=*,movie_people(role,movies(id,title,poster_path))"
                );

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
            catch (Exception ex)
            {
                MessageBox.Show($"LoadPeople ERROR: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // Card builder
        // -------------------------------------------------------------------
        private Panel CreateCard(string text, string imageUrl)
        {
            var card = new Panel
            {
                Width = 150,
                Height = 283,
                BackColor = Color.FromArgb(36, 38, 69),
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 5, 0)
            };

            var poster = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 225,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            poster.Paint += Poster_Paint;

            _ = LoadImageAsync(poster, imageUrl);

            var label = new Label
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 0, 0, 0),
                AutoSize = false,
                Height = 40,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter
            };

            poster.Controls.Add(label);
            card.Controls.Add(poster);

            return card;
        }

        private void Poster_Paint(object sender, PaintEventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb == null) return;

            var rect = new Rectangle(0, 0, pb.Width - 1, pb.Height - 1);
            using (var pen = new Pen(Color.FromArgb(255, 27, 29, 54), 5))
                e.Graphics.DrawRectangle(pen, rect);
        }

        private static void AttachClickRecursive(Control ctrl, Action onClick)
        {
            ctrl.Click += (s, e) => onClick();
            foreach (Control child in ctrl.Controls)
                AttachClickRecursive(child, onClick);
        }

        private static async Task LoadImageAsync(PictureBox box, string url)
        {
            try
            {
                var bytes = await Http.GetByteArrayAsync(url);
                var ms = new MemoryStream(bytes);
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
            if (card?.Tag is not Movies movie) return;

            string posterPath = $"{SupabaseUrl}/storage/v1/object/public/pictures/{movie.poster_path}";
            var genreNames = movie.movie_genres?
                .Select(mg => mg.genres?.name)
                .Where(n => n != null)
                .ToList();
            string genres = string.Join(", ", genreNames ?? new List<string>());

            new MoviePage(
                movie.title, movie.duration_minutes, movie.rating, movie.release_year,
                movie.description, posterPath, movie.status, movie.adult,
                movie.director, genres, movie.movie_people
            ).Show();

            Hide();
        }

        private void PersonCard_Click(Panel card)
        {
            if (card?.Tag is not Person person) return;

            string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";

            new PersonPage(
                person.name, person.info, imageUrl,
                person.date_of_birth, person.place_of_birth, person.gender,
                person.movie_people
            ).Show();

            Hide();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
    }
}