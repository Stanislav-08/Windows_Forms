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
    public partial class ProfilePage : Form
    {
        private static HttpClient Http = AppHttpClient.Instance;
        private static SupabaseClient Supabase = new SupabaseClient();
        private const string SupabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        public ProfilePage()
        {
            InitializeComponent();

            // NavigationBar
            NavigationBar navigationBar = new NavigationBar("profile");
            navigationBar.Dock = DockStyle.Left;
            Controls.Add(navigationBar);

            // TitleBar
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            // Set username
            usernameLabel.Text = Session.DisplayName ?? "Guest";

            // Configure flow panel
            ConfigureFlow(watchlistFlowLayoutPanel);

            // Load watchlist if logged in
            if (Session.IsLoggedIn)
            {
              Task loadWatchlist = LoadWatchlist();
            }
            else
            {
                usernameLabel.Text = "Guest";
            }
        }

        //----------Flow panel configuration----------

        private static void ConfigureFlow(FlowLayoutPanel flow)
        {
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.WrapContents = false;
            flow.AutoScroll = true;
            flow.HorizontalScroll.Visible = false;
            flow.VerticalScroll.Visible = false;
        }
        //----------Watchlist loading function----------
        private async Task LoadWatchlist()
        {
            try
            {
                var watchlist = await Supabase.GetWatchlist(Session.AccessToken!);

                watchlistFlowLayoutPanel.SuspendLayout();
                watchlistFlowLayoutPanel.Controls.Clear();

                foreach (var entry in watchlist)
                {
                    if (entry.movies == null) continue;

                    string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{entry.movies.poster_path}";
                    var card = CreateCard(entry.movies.title, imageUrl);
                    card.Tag = entry.movies;
                    AttachClickRecursive(card, () => MovieCard_Click(card));
                    watchlistFlowLayoutPanel.Controls.Add(card);
                }

                watchlistFlowLayoutPanel.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"LoadWatchlist ERROR: {ex.Message}");
            }
        }

        //----------Watchlist card creation----------

        private Panel CreateCard(string text, string imageUrl)
        {
            //Card
            var card = new Panel
            {
                Width = 150,
                Height = 283,
                BackColor = Color.FromArgb(36, 38, 69),
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 5, 0)
            };

            //Poster
            var poster = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 225,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            //Adding paint and loading image
            poster.Paint += Poster_Paint;
            var loadImage = LoadImageAsync(poster, imageUrl);

            //Label
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

            //Adding controls
            poster.Controls.Add(label);
            card.Controls.Add(poster);

            return card;
        }

        //----------Card border function----------
        private void Poster_Paint(object sender, PaintEventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb == null) return;

            var rect = new Rectangle(0, 0, pb.Width - 1, pb.Height - 1);
            using (var pen = new Pen(Color.FromArgb(255, 27, 29, 54), 5))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        //----------Click addition function---------- ???
        private static void AttachClickRecursive(Control ctrl, Action onClick)
        {
            ctrl.Click += (s, e) => onClick();
            foreach (Control child in ctrl.Controls)
                AttachClickRecursive(child, onClick);
        }

        //----------Image loading function----------
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
                {
                    box.BackColor = Color.DarkGray;
                }
            }
        }

        //----------Card click function----------
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
                movie.director, genres, movie.movie_people, movie.id
            ).Show();

            Hide();
        }
    }
}