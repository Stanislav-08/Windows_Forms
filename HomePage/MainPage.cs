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
        public MainPage()
        {
            InitializeComponent();

            // Title Bar
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);

            ConfigureFlow(flowLayoutPanel1);
            ConfigureFlow(flowLayoutPanel2);
        }

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

        static AuthenticationService db = new AuthenticationService();
        static HttpClient http = new HttpClient();

        private async Task LoadMovies()
        {
            var movies = await db.GetDatabase<Movie>("movies") ?? new List<Movie>();

            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (var movie in movies)
            {
                var card = CreateCard(movie.title, movie.url);

                card.Tag = movie;
                card.Click += MovieCard_Click;

                flowLayoutPanel1.Controls.Add(card);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        private async Task LoadPeople()
        {
            var people = await db.GetDatabase<Person>("people") ?? new List<Person>();

            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel2.Controls.Clear();

            foreach (var person in people)
            {
                var card = CreateCard(person.name, person.url);

                card.Tag = person;
                card.Click += PersonCard_Click;

                flowLayoutPanel2.Controls.Add(card);
            }

            flowLayoutPanel2.ResumeLayout();
        }

        private Panel CreateCard(string text, string imageUrl)
        {
            Panel card = new Panel();
            card.Width = 150;
            card.Height = 100;
            card.BackColor = Color.FromArgb(30, 30, 30);
            card.Cursor = Cursors.Hand;
            card.Margin = new Padding(10);

            PictureBox poster = new PictureBox();
            poster.Dock = DockStyle.Top;
            poster.Height = 70;
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

        private Panel GetCard(Control ctrl)
        {
            while (ctrl != null && ctrl is not Panel)
                ctrl = ctrl.Parent;

            return ctrl as Panel;
        }

        private void MovieCard_Click(object sender, EventArgs e)
        {
            var card = GetCard(sender as Control);
            if (card?.Tag is not Movie movie) return;

            new MoviePage(movie.title, movie.description, movie.url).ShowDialog();
        }

        private void PersonCard_Click(object sender, EventArgs e)
        {
            var card = GetCard(sender as Control);
            if (card?.Tag is not Person person) return;

            new PersonPage(person.name, person.info, person.url).ShowDialog();
        }
    }
}