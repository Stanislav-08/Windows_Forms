using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();

            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            flowLayoutPanel1.VerticalScroll.Visible = false;
            flowLayoutPanel1.HorizontalScroll.Minimum = 0;
            flowLayoutPanel1.HorizontalScroll.Maximum = 0;

            flowLayoutPanel2.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel2.WrapContents = false;
            flowLayoutPanel2.AutoScroll = true;
            flowLayoutPanel2.HorizontalScroll.Visible = false;
            flowLayoutPanel2.VerticalScroll.Visible = false;
        }

        private async void MainPage_Load(object sender, EventArgs e)
        {
            await LoadMovies();
            await LoadPeople();
        }

        static AuthenticationService db = new AuthenticationService();

        private async Task LoadMovies()
        {
            var movies = await db.GetDatabase<Movie>("movies") ?? new List<Movie>();

            flowLayoutPanel1.Controls.Clear();

            foreach (var movie in movies)
            {
                Panel card = new Panel();
                card.Width = 150;
                card.Height = 100;
                card.BackColor = Color.FromArgb(30, 30, 30);

                PictureBox poster = new PictureBox();
                poster.Dock = DockStyle.Top;
                poster.Height = 70;
                poster.SizeMode = PictureBoxSizeMode.StretchImage;

                try { poster.Load(movie.url); }
                catch { poster.BackColor = Color.DarkGray; }

                Label title = new Label();
                title.Text = movie.title;
                title.Dock = DockStyle.Bottom;
                title.ForeColor = Color.White;
                title.TextAlign = ContentAlignment.MiddleCenter;

                card.Controls.Add(title);
                card.Controls.Add(poster);

                card.Tag = movie;
                card.Cursor = Cursors.Hand;

                card.Click += MovieCard_Click;
                poster.Click += MovieCard_Click;
                title.Click += MovieCard_Click;

                flowLayoutPanel1.Controls.Add(card);
            }
        }

        private async Task LoadPeople()
        {
            var people = await db.GetDatabase<Person>("people") ?? new List<Person>();
            flowLayoutPanel2.Controls.Clear();

            foreach (var person in people)
            {
                Panel card = new Panel();
                card.Width = 150;
                card.Height = 100;
                card.BackColor = Color.FromArgb(30, 30, 30);

                PictureBox poster = new PictureBox();
                poster.Dock = DockStyle.Top;
                poster.Height = 70;
                poster.SizeMode = PictureBoxSizeMode.StretchImage;

                try { poster.Load(person.url); }
                catch { poster.BackColor = Color.DarkGray; }

                Label name = new Label();
                name.Text = person.name;
                name.Dock = DockStyle.Bottom;
                name.ForeColor = Color.White;
                name.TextAlign = ContentAlignment.MiddleCenter;

                card.Controls.Add(name);
                card.Controls.Add(poster);

                card.Tag = person;
                card.Cursor = Cursors.Hand;

                card.Click += PersonCard_Click;
                poster.Click += PersonCard_Click;
                name.Click += PersonCard_Click;

                flowLayoutPanel2.Controls.Add(card);
            }
        }

        private void MovieCard_Click(object sender, EventArgs e)
        {
            if (sender is not Control ctrl) return;

            Panel card = ctrl as Panel ?? ctrl.Parent as Panel;
            if (card?.Tag is not Movie movie) return;

            new MoviePage(movie.title, movie.description, movie.url).ShowDialog();
        }

        private void PersonCard_Click(object sender, EventArgs e)
        {
            if (sender is not Control ctrl) return;

            Panel card = ctrl as Panel ?? ctrl.Parent as Panel;
            if (card?.Tag is not Person person) return;

            new PersonPage(person.name, person.info, person.url).ShowDialog();
        }
    }
}