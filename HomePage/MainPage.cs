using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            flowLayoutPanel1.Dock = DockStyle.Fill;
        }

        private async void MainPage_Load(object sender, EventArgs e)
        {
            AuthenticationService db = new AuthenticationService();

            var movies = await db.GetMovies();

            flowLayoutPanel1.Controls.Clear();

            foreach (var movie in movies)
            {
                Panel card = new Panel();
                card.Width = 150;
                card.Height = 220;
                card.Margin = new Padding(10);
                card.BackColor = Color.FromArgb(30, 30, 30);

                PictureBox poster = new PictureBox();
                poster.Dock = DockStyle.Top;
                poster.Height = 170;
                poster.SizeMode = PictureBoxSizeMode.StretchImage;

                try
                {
                    poster.Load(movie.url);
                }
                catch
                {
                    poster.BackColor = Color.DarkGray;
                }

                Label title = new Label();
                title.Text = movie.title;
                title.Dock = DockStyle.Bottom;
                title.ForeColor = Color.White;
                title.TextAlign = ContentAlignment.MiddleCenter;

                card.Controls.Add(title);
                card.Controls.Add(poster);

                flowLayoutPanel1.Controls.Add(card);
            }
        }
    }
}
