using App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace App
{
    public partial class MoviePage : Form
    {
        private static readonly HttpClient http = AppHttpClient.Instance; // shared client
        private string state1 = "outlined", state2 = "outlined", state3 = "outlined", state4 = "outlined", state5 = "outlined";

        public MoviePage(string title, int durationMinutes, double rating, int releaseYear, string description, string posterPath, string status, bool adult, string director, string genres)
        {
            InitializeComponent();

            //Title Bar 
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);
            LoadImageAsync(pictureBox1, posterPath);
            label1.Text = $"Release year:\n{releaseYear.ToString()}";
            label2.Text = durationMinutes.ToString();
            label3.Text = title;
            label6.Text = description;
            label7.Text = $"Genres: {genres}";
            label8.Text = director;
            button5.Image = Icons.Get("outlined_star");
            button1.Image = Icons.Get("outlined_star");
            button6.Image = Icons.Get("outlined_star");
            button7.Image = Icons.Get("outlined_star");
            button8.Image = Icons.Get("outlined_star");
            button2.Image = Icons.Get("favourite");

        }
        private async Task LoadImageAsync(PictureBox pictureBox, string url)
        {
            try
            {
                var bytes = await http.GetByteArrayAsync(url);
                var ms = new MemoryStream(bytes);
                pictureBox.Image = Image.FromStream(ms);
            }
            catch
            {
                if (!pictureBox.IsDisposed)
                    pictureBox.BackColor = Color.DarkGray;
            }
        }
        private string ChangeRating(Button button, string state)
        {
            if (state == "outlined")
                state = "half_filled";
            else if (state == "half_filled")
                state = "filled";
            else
                state = "outlined";

            button.Image = Icons.Get(state + "_star");
            return state;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //tochka razdelqshta label 1-2-3
        }

        private void button5_Click(object sender, EventArgs e)
        {
            state1 = ChangeRating(button5, state1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            state2 = ChangeRating(button1, state2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            state3 = ChangeRating(button6, state3);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            state4=ChangeRating(button7, state4);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            state5=ChangeRating(button8, state5);
        }
    }
}
