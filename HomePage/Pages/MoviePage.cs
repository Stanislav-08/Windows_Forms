using App.Databases;
using App.Services;

namespace App
{
    public partial class MoviePage : Form
    {
        private static HttpClient Http = AppHttpClient.Instance;
        private static SupabaseClient Supabase = new SupabaseClient();
        private string SupabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        private string[] starStates = { "outlined", "outlined", "outlined", "outlined", "outlined" };
        private Button[] starButtons;
        private double avgRating;
        private bool isInWatchlist = false;
        private long movieId;

        public MoviePage(string title, int durationMinutes, double rating, int releaseYear,
                         string description, string posterPath, string status, bool adult,
                         string director, string genres, List<MoviePerson>? crew = null,
                         long movieId = 0)
        {
            InitializeComponent();

            //Title bar
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            Controls.Add(titleBar);

            var loadImage = LoadImageAsync(pictureBox1, posterPath);

            //Assigning values
            this.movieId = movieId;
            this.avgRating = rating;
            label1.Text = releaseYear.ToString();
            label2.Text = $"{durationMinutes} min";
            label3.Text = title;
            label6.Text = description;
            label7.Text = genres;
            label8.Text = director;

            button9.Image = Icons.Get("back_arrow");

            starButtons = new Button[] { button5, button1, button6, button7, button8 };

            foreach (var btn in starButtons)
            {
                btn.Image = Icons.Get("outlined_star");
            }

            if (Session.IsLoggedIn)
            {
                for (int i = 0; i < starButtons.Length; i++)
                {
                    int index = i;
                    starButtons[i].Click += (s, e) => StarClicked(index);
                }
            }
            else
            {
                DisplayAverageRating();
            }

            //Watchlist button — hidden for guests
            button2.Image = Icons.Get("outlined_favourite", 60);
            button2.Visible = Session.IsLoggedIn;
            label13.Visible = Session.IsLoggedIn;

            ConfigureFlow(flowLayoutPanel1);
            LoadCrew(crew);
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

        //----------Loading crew function----------

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
                card.Tag = member.people;
                AttachClickRecursive(card, () => CrewCard_Click(card));
                flowLayoutPanel1.Controls.Add(card);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        //----------Card creation----------

        private Panel CreateCrewCard(string name, string role, string imageUrl)
        {
            var card = new Panel
            {
                Width = 122,
                Height = 183,
                BackColor = Color.FromArgb(36, 38, 69),
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 5, 0)
            };

            var photo = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = card.Height,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            var loadImage = LoadImageAsync(photo, imageUrl);

            var nameLabel = new Label
            {
                Text = name,
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

            photo.Controls.Add(nameLabel);
            photo.Controls.Add(roleLabel);
            card.Controls.Add(photo);

            return card;
        }

        //----------Click function----------

        private async Task CrewCard_Click(Panel card)
        {
            if (card?.Tag is not Person partialPerson) return;

            //Fetch full person data by id
            var person = await Supabase.GetPersonById(partialPerson.id);
            if (person == null) return;

            string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";

            new PersonPage(
                person.name, person.info, imageUrl,
                person.date_of_birth, person.place_of_birth, person.gender,
                person.movie_people
            ).Show();

            Hide();
        }

        //----------Click addition function----------

        private static void AttachClickRecursive(Control ctrl, Func<Task> onClick)
        {
            ctrl.Click += async (s, e) => await onClick();
            foreach (Control child in ctrl.Controls)
            {
                AttachClickRecursive(child, onClick);
            }
        }

        //----------Image loading function----------

        private static async Task LoadImageAsync(PictureBox pictureBox, string url)
        {
            try
            {
                var bytes = await Http.GetByteArrayAsync(url);
                var ms = new MemoryStream(bytes);
                pictureBox.Image = Image.FromStream(ms);
            }
            catch
            {
                if (!pictureBox.IsDisposed)
                {
                    pictureBox.BackColor = Color.DarkGray;
                }
            }
        }

        //----------Display rating for guest----------

        private void DisplayAverageRating()
        {
            double mapped = avgRating / 2.0;
            double rounded = Math.Round(mapped * 2, MidpointRounding.AwayFromZero) / 2.0;

            for (int i = 0; i < starButtons.Length; i++)
            {
                double starValue = rounded - i;
                string state;

                if (starValue >= 1.0) state = "filled";
                else if (starValue >= 0.5) state = "half_filled";
                else state = "outlined";

                starStates[i] = state;
                starButtons[i].Image = Icons.Get(state + "_star");
            }
        }

        //----------Star state logic----------

        private void StarClicked(int clickedIndex)
        {
            //Cycle the clicked star
            string current = starStates[clickedIndex];
            string next = current == "outlined" ? "half_filled" : current == "half_filled" ? "filled" : "outlined";

            //If stars back to outlined, clear all
            if (next == "outlined")
            {
                for (int i = 0; i < starButtons.Length; i++)
                {
                    starStates[i] = "outlined";
                    starButtons[i].Image = Icons.Get("outlined_star");
                }
                return;
            }

            //Fill all stars before the clicked one
            for (int i = 0; i < clickedIndex; i++)
            {
                starStates[i] = "filled";
                starButtons[i].Image = Icons.Get("filled_star");
            }

            starStates[clickedIndex] = next;
            starButtons[clickedIndex].Image = Icons.Get(next + "_star");

            //Clear all stars after the clicked one
            for (int i = clickedIndex + 1; i < starButtons.Length; i++)
            {
                starStates[i] = "outlined";
                starButtons[i].Image = Icons.Get("outlined_star");
            }
        }

        //----------Watchlist button function----------

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!isInWatchlist)
            {
                bool success = await Supabase.AddToWatchlist(movieId, Session.AccessToken!);
                if (success)
                {
                    isInWatchlist = true;
                    button2.Image = Icons.Get("filled_favourite", 60);
                }
                else
                {
                    MessageBox.Show("Failed to add to watchlist.");
                }
            }
            else
            {
                bool success = await Supabase.RemoveFromWatchlist(movieId, Session.AccessToken!);
                if (success)
                {
                    isInWatchlist = false;
                    button2.Image = Icons.Get("outlined_favourite", 60);
                }
                else
                {
                    MessageBox.Show("Failed to remove from watchlist.");
                }
            }
        }

        //----------Back button function----------

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainPage().Show();
        }
    }
}