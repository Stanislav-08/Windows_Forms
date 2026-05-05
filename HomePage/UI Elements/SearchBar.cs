using App.Databases;
using App.Services;

namespace App.UI_Elements
{
    public partial class SearchBar : UserControl
    {
        private static SupabaseClient Supabase = new SupabaseClient();
        private string SupabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";

        //Holds all movies and people loaded once
        private List<Movies> allMovies = new List<Movies>();
        private List<Person> allPeople = new List<Person>();

        //Currently selected result
        private object selectedResult = null;

        private Panel dropdown;

        public SearchBar(Point location, int width)
        {
            InitializeComponent();

            //Add design data
            this.Location = location;
            this.Width = width;
            SearchBarTextBox.Width = width - SearchButton.Width;

            // Build dropdown panel
            dropdown = new Panel
            {
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(28, 30, 54),
                AutoScroll = true
            };

            //Add events
            SearchBarTextBox.TextChanged += SearchBarTextBox_TextChanged;
            SearchButton.Click += SearchButton_Click;

            //Hide dropdown 
            SearchBarTextBox.LostFocus += async (s, e) =>
            {
                await Task.Delay(150);
                if (!IsDropdownFocused())
                {
                    dropdown.Visible = false;
                }
            };

            var loadData = LoadData();
        }

        //----------Moves the search bar to front----------
        //!!!Nothing is clipped!!!
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null && !Parent.Controls.Contains(dropdown))
            {
                dropdown.Width = this.Width;
                dropdown.Left = this.Left;
                dropdown.Top = this.Top + this.Height;
                Parent.Controls.Add(dropdown);
                dropdown.BringToFront();
            }
        }

        //----------Data loading function----------

        private async Task LoadData()
        {
            try
            {
                var movies = await Supabase.GetAll<Movies>(
                    "movies?select=id,title,poster_path,duration_minutes,rating,release_year,description," +
                    "status,adult,director,movie_genres(genres(name)),movie_people(role,people(name,profile_path))"
                );
                var people = await Supabase.GetAll<Person>(
                    "people?select=*,movie_people(role,movies(id,title,poster_path))"
                );

                // Ensure we're on the UI thread before assigning
                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        allMovies = movies;
                        allPeople = people;
                    });
                }
                else
                {
                    allMovies = movies;
                    allPeople = people;
                }
            }
            catch { }
        }

        //----------Search logic----------

        private void SearchBarTextBox_TextChanged(object sender, EventArgs e)
        {
            string query = SearchBarTextBox.Text.Trim();
            selectedResult = null;

            if (string.IsNullOrEmpty(query))
            {
                dropdown.Visible = false;
                return;
            }

            //Find top 5 matches
            var movieMatches = allMovies
                .Where(m => m.title != null && m.title.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                .Take(3)
                .Select(m => (object)m);

            var peopleMatches = allPeople
                .Where(p => p.name != null && p.name.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                .Take(2)
                .Select(p => (object)p);

            //If not enough, fall back to containing letters
            if (!movieMatches.Any() && !peopleMatches.Any())
            {
                movieMatches = allMovies
                    .Where(m => m.title != null && m.title.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Take(3)
                    .Select(m => (object)m);

                peopleMatches = allPeople
                    .Where(p => p.name != null && p.name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Take(2)
                    .Select(p => (object)p);
            }

            var results = movieMatches.Concat(peopleMatches).Take(5).ToList();

            ShowDropdown(results);
        }

        //----------Dropdown design----------

        private void ShowDropdown(List<object> results)
        {
            dropdown.Controls.Clear();

            if (results.Count == 0)
            {
                dropdown.Visible = false;
                return;
            }

            int itemHeight = 40;
            int y = 0;

            foreach (var result in results)
            {
                string name = result is Movies m ? m.title : ((Person)result).name;
                string type = result is Movies ? "🎬 Movie" : "👤 Person";

                var item = new Panel
                {
                    Width = dropdown.Width - 4,
                    Height = itemHeight,
                    Top = y,
                    Left = 0,
                    Cursor = Cursors.Hand,
                    BackColor = Color.FromArgb(36, 38, 54),
                    Tag = result
                };

                var typeLabel = new Label
                {
                    Text = type,
                    ForeColor = Color.Gray,
                    Font = new Font(Font.FontFamily, 7f),
                    Left = 8,
                    Top = 2,
                    AutoSize = true
                };

                var nameLabel = new Label
                {
                    Text = name,
                    ForeColor = Color.White,
                    Left = 8,
                    Top = 16,
                    AutoSize = true
                };

                item.Controls.Add(typeLabel);
                item.Controls.Add(nameLabel);

                //Hover effect
                item.MouseEnter += (s, e) => item.BackColor = Color.FromArgb(50, 52, 80);
                item.MouseLeave += (s, e) => item.BackColor = Color.FromArgb(36, 38, 54);
                typeLabel.MouseEnter += (s, e) => item.BackColor = Color.FromArgb(50, 52, 80);
                typeLabel.MouseLeave += (s, e) => item.BackColor = Color.FromArgb(36, 38, 54);
                nameLabel.MouseEnter += (s, e) => item.BackColor = Color.FromArgb(50, 52, 80);
                nameLabel.MouseLeave += (s, e) => item.BackColor = Color.FromArgb(36, 38, 54);

                //Click selects the result
                Action selectItem = () =>
                {
                    selectedResult = item.Tag;
                    SearchBarTextBox.TextChanged -= SearchBarTextBox_TextChanged;
                    SearchBarTextBox.Text = name;
                    SearchBarTextBox.TextChanged += SearchBarTextBox_TextChanged;
                    dropdown.Visible = false;
                };

                item.Click += (s, e) => selectItem();
                nameLabel.Click += (s, e) => selectItem();
                typeLabel.Click += (s, e) => selectItem();

                dropdown.Controls.Add(item);
                y += itemHeight;
            }

            dropdown.Height = Math.Min(y, 200);
            dropdown.Visible = true;
            dropdown.BringToFront();
        }

        private bool IsDropdownFocused()
        {
            foreach (Control c in dropdown.Controls)
            {
                if (c.Focused)
                {
                    return true;
                }
            }
                return false;
        }

        //---------Search function----------

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (selectedResult == null) return; // nothing selected — do nothing

            dropdown.Visible = false;

            if (selectedResult is Movies movie)
            {
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
                this.FindForm().Hide();

            }
            else if (selectedResult is Person person)
            {
                string imageUrl = $"{SupabaseUrl}/storage/v1/object/public/pictures/{person.profile_path}";
                new PersonPage(
                    person.name, person.info, imageUrl,
                    person.date_of_birth, person.place_of_birth, person.gender,
                    person.movie_people
                ).Show();
                this.FindForm().Hide();
            }
        }
    }
}