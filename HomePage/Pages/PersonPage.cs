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
    public partial class PersonPage : Form
    {
        private static readonly HttpClient Http = AppHttpClient.Instance;
        private const string SupabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        public PersonPage(string name, string info, string profilePath,
                          string dateOfBirth, string placeOfBirth, string gender,
                          List<MoviePerson>? movies = null)
        {
            InitializeComponent();

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            button9.Image = Icons.Get("back_arrow");

            // Fill labels
            label3.Text = name;
            label7.Text = string.IsNullOrEmpty(dateOfBirth) ? "Unknown" : dateOfBirth;
            label6.Text = string.IsNullOrEmpty(info) ? "No biography available." : info;
            label9.Text = string.IsNullOrEmpty(gender) ? "Unknown" : gender;
            label4.Text = string.IsNullOrEmpty(placeOfBirth) ? "Unknown" : placeOfBirth;

            // Load profile picture
            _ = LoadImageAsync(pictureBox1, profilePath);

            // Configure and load movie flow panel
            ConfigureFlow(flowLayoutPanel1);
            LoadMovies(movies);
        }

        // ===================================================================
        // FLOW PANEL
        // ===================================================================

        private static void ConfigureFlow(FlowLayoutPanel flow)
        {
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.WrapContents = false;
            flow.AutoScroll = true;
            flow.HorizontalScroll.Visible = false;
            flow.VerticalScroll.Visible = false;
        }

        private void LoadMovies(List<MoviePerson>? movies)
        {
            if (movies == null || movies.Count == 0) return;

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var mp in movies)
            {
                if (mp.movies == null) continue;

                string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{mp.movies.poster_path}";
                var card = CreateMovieCard(mp.movies.title, mp.role, imageUrl);
                card.Tag = mp.movies;
                AttachClickRecursive(card, () => MovieCard_Click(card));
                flowLayoutPanel1.Controls.Add(card);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        private Panel CreateMovieCard(string title, string role, string imageUrl)
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
            _ = LoadImageAsync(poster, imageUrl);

            var titleLabel = new Label
            {
                Text = title,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 0, 0, 0),
                AutoSize = false,
                Height = 30,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var roleLabel = new Label
            {
                Text = role,
                ForeColor = Color.LightGray,
                BackColor = Color.FromArgb(180, 0, 0, 0),
                AutoSize = false,
                Height = 25,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.FontFamily, 7f)
            };

            poster.Controls.Add(titleLabel);
            poster.Controls.Add(roleLabel);
            card.Controls.Add(poster);

            return card;
        }

        // ===================================================================
        // NAVIGATION
        // ===================================================================

        private void MovieCard_Click(Panel card)
        {
            if (card?.Tag is not Movies movie) return;

            string posterPath = $"{SupabaseUrl}/storage/v1/object/public/pictures/{movie.poster_path}";
            var genreNames = movie.movie_genres?
                .Select(mg => mg.genres?.name)
                .Where(n => n != null)
                .ToList() ?? new System.Collections.Generic.List<string>();
            string genres = string.Join(", ", genreNames);

            new MoviePage(
                movie.title, movie.duration_minutes, movie.rating, movie.release_year,
                movie.description, posterPath, movie.status, movie.adult,
                movie.director, genres, movie.movie_people
            ).Show();

            Hide();
        }

        private static void AttachClickRecursive(Control ctrl, Action onClick)
        {
            ctrl.Click += (s, e) => onClick();
            foreach (Control child in ctrl.Controls)
                AttachClickRecursive(child, onClick);
        }

        // ===================================================================
        // IMAGE LOADING
        // ===================================================================

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

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainPage().Show();
        }
    }
}