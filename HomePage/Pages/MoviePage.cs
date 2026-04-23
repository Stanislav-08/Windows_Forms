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

        private void label4_Click(object sender, EventArgs e)
        {
            //tochka razdelqshta label 1-2-3
        }
    }
}
