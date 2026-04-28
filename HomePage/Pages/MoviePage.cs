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
    public partial class MoviePage : Form
    {
        private static readonly HttpClient _http = AppHttpClient.Instance;
        private const string SupabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        private string state1 = "outlined", state2 = "outlined", state3 = "outlined",
                       state4 = "outlined", state5 = "outlined";
        private string stateFavourite = "not favourite";

        public MoviePage(string title, int durationMinutes, double rating, int releaseYear,
                         string description, string posterPath, string status, bool adult,
                         string director, string genres, List<MoviePerson>? crew = null)
        {
            InitializeComponent();

            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            LoadImageAsync(pictureBox1, posterPath);

            label1.Text = releaseYear.ToString();
            label2.Text = $"{durationMinutes} min";
            label3.Text = title;
            label6.Text = description;
            label7.Text = genres;
            label8.Text = director;

            button5.Image = Icons.Get("outlined_star");
            button1.Image = Icons.Get("outlined_star");
            button6.Image = Icons.Get("outlined_star");
            button7.Image = Icons.Get("outlined_star");
            button8.Image = Icons.Get("outlined_star");
            button2.Image = Icons.Get("outlined_favourite", 60);

            ConfigureFlow(flowLayoutPanel1);
            LoadCrew(crew);
        }

        // ===================================================================
        // CREW
        // ===================================================================

        private static void ConfigureFlow(FlowLayoutPanel flow)
        {
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.WrapContents = false;
            flow.AutoScroll = true;
            flow.HorizontalScroll.Visible = false;
            flow.VerticalScroll.Visible = false;
        }

        private void LoadCrew(List<MoviePerson>? crew)
        {
            if (crew == null || crew.Count == 0) return;

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var member in crew)
            {
                if (member.people == null) continue;

                string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{member.people.profile_path}";
                var card = CreateCrewCard(member.people.name, member.role, imageUrl);
                flowLayoutPanel1.Controls.Add(card);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        private Panel CreateCrewCard(string name, string role, string imageUrl)
        {
            var card = new Panel
            {
                Width = 100,
                Height = 160,
                BackColor = Color.FromArgb(36, 38, 69),
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };

            var photo = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 100,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            _ = LoadImageAsync(photo, imageUrl);

            var nameLabel = new Label
            {
                Text = name,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30
            };

            var roleLabel = new Label
            {
                Text = role,
                Dock = DockStyle.Bottom,
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 25
            };

            card.Controls.Add(roleLabel);
            card.Controls.Add(nameLabel);
            card.Controls.Add(photo);

            return card;
        }

        // ===================================================================
        // IMAGE LOADING
        // ===================================================================

        private static async Task LoadImageAsync(PictureBox pictureBox, string url)
        {
            try
            {
                var bytes = await _http.GetByteArrayAsync(url);
                var ms = new MemoryStream(bytes);
                pictureBox.Image = Image.FromStream(ms);
            }
            catch
            {
                if (!pictureBox.IsDisposed)
                    pictureBox.BackColor = Color.DarkGray;
            }
        }

        // ===================================================================
        // RATING
        // ===================================================================

        private string ChangeRating(Button button, string state)
        {
            state = state == "outlined" ? "half_filled" :
                    state == "half_filled" ? "filled" : "outlined";

            button.Image = Icons.Get(state + "_star");
            return state;
        }

        private void button5_Click(object sender, EventArgs e) => state1 = ChangeRating(button5, state1);
        private void button1_Click(object sender, EventArgs e) => state2 = ChangeRating(button1, state2);
        private void button6_Click(object sender, EventArgs e) => state3 = ChangeRating(button6, state3);
        private void button7_Click(object sender, EventArgs e) => state4 = ChangeRating(button7, state4);
        private void button8_Click(object sender, EventArgs e) => state5 = ChangeRating(button8, state5);

        // ===================================================================
        // FAVOURITE
        // ===================================================================

        private void button2_Click(object sender, EventArgs e)
        {
            if (stateFavourite == "not favourite")
            {
                stateFavourite = "favourite";
                button2.Image = Icons.Get("filled_favourite", 60);
            }
            else
            {
                stateFavourite = "not favourite";
                button2.Image = Icons.Get("outlined_favourite", 60);
            }
        }
    }
}