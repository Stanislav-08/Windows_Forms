using App.Databases;
using App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class AdminMainPage : Form
    {
        private static SupabaseClient Supabase = new SupabaseClient();

        public AdminMainPage()
        {
            InitializeComponent();

            //Title bar
            TitleBar titleBar = new TitleBar();
            titleBar.Dock = DockStyle.Top;
            this.Controls.Add(titleBar);
        }

        private async void AdminMainPage_Load(object sender, EventArgs e)
        {
            await LoadMovies();
            await LoadPeople();
        }

        //----------Movies grid----------

        private async Task LoadMovies()
        {
            var data = await Supabase.GetAll<Movies>(
                "movies?select=id,title,rating,release_year,duration_minutes,status,adult,director"
            );

            var flat = data.Select(m => new
            {
                m.id,
                m.title,
                m.rating,
                m.release_year,
                m.duration_minutes,
                m.status,
                m.adult,
                m.director
            }).ToList();

            dataGridView1.Invoke(() =>
            {
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = flat;
            });
        }

        //----------People grid----------

        private async Task LoadPeople()
        {
            var data = await Supabase.GetAll<Person>(
                "people?select=id,name,gender,date_of_birth,place_of_birth"
            );

            var flat = data.Select(p => new
            {
                p.id,
                p.name,
                p.gender,
                p.date_of_birth,
                p.place_of_birth
            }).ToList();

            dataGridView2.Invoke(() =>
            {
                dataGridView2.AutoGenerateColumns = true;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = flat;
            });
        }

        //----------File parser----------

        private Dictionary<string, string> ParseFile(string path)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (!line.Contains('=')) continue;

                var parts = line.Split('=', 2);
                dict[parts[0].Trim()] = parts[1].Trim();
            }

            return dict;
        }

        //----------Import movie----------

        private async Task ImportMovie(Dictionary<string, string> d)
        {
            var payload = new
            {
                title = d["title"],
                duration_minutes = short.Parse(d["duration_minutes"]),
                description = d["description"],
                rating = double.Parse(d["rating"]),
                release_year = short.Parse(d["release_year"]),
                poster_path = d["poster_path"],
                status = d["status"],
                adult = bool.Parse(d["adult"]),
                director = d["director"]
            };

            await Supabase.Insert("movies", payload);
        }

        //----------Import person----------

        private async Task ImportPerson(Dictionary<string, string> d)
        {
            var payload = new
            {
                name = d["name"],
                info = d["info"],
                profile_path = d["profile_path"],
                date_of_birth = DateTime.Parse(d["date_of_birth"]),
                gender = d["gender"],
                place_of_birth = d["place_of_birth"]
            };

            await Supabase.Insert("people", payload);
        }

        //----------Export movie----------

        private string MovieToText(Movies m)
        {
            return $"title={m.title}\n" +
                   $"duration_minutes={m.duration_minutes}\n" +
                   $"description={m.description}\n" +
                   $"rating={m.rating}\n" +
                   $"release_year={m.release_year}\n" +
                   $"poster_path={m.poster_path}\n" +
                   $"status={m.status}\n" +
                   $"adult={m.adult.ToString().ToLower()}\n" +
                   $"director={m.director}";
        }

        //----------Export person----------

        private string PersonToText(Person p)
        {
            return $"name={p.name}\n" +
                   $"info={p.info}\n" +
                   $"profile_path={p.profile_path}\n" +
                   $"date_of_birth={p.date_of_birth}\n" +
                   $"gender={p.gender}\n" +
                   $"place_of_birth={p.place_of_birth}";
        }

        //----------Import button----------

        private async void button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select import file";
            ofd.Filter = "Text files (*.txt)|*.txt";

            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var data = ParseFile(ofd.FileName);

                if (data.ContainsKey("title"))
                {
                    await ImportMovie(data);
                    await LoadMovies();
                    MessageBox.Show("Movie imported successfully.");
                }
                else if (data.ContainsKey("name"))
                {
                    await ImportPerson(data);
                    await LoadPeople();
                    MessageBox.Show("Person imported successfully.");
                }
                else
                {
                    MessageBox.Show("Could not determine file type. Make sure the file has a 'title' or 'name' field.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed: {ex.Message}");
            }
        }

        //----------Export button----------

        private async void button2_Click(object sender, EventArgs e)
        {
            //Ask what to export
            var choice = MessageBox.Show(
                "Click Yes to export a Movie, No to export a Person.",
                "Export",
                MessageBoxButtons.YesNoCancel
            );

            if (choice == DialogResult.Cancel) return;

            try
            {
                string content;

                if (choice == DialogResult.Yes)
                {
                    //Get all movies and let user pick by title
                    var movies = await Supabase.GetAll<Movies>(
                        "movies?select=id,title,duration_minutes,description,rating,release_year,poster_path,status,adult,director"
                    );

                    var titles = movies.Select(m => m.title).ToArray();
                    if (titles.Length == 0) { MessageBox.Show("No movies found."); return; }

                    var picker = new Form();
                    picker.Text = "Select Movie";
                    picker.Width = 350;
                    picker.Height = 200;
                    picker.StartPosition = FormStartPosition.CenterParent;

                    var list = new ListBox { Dock = DockStyle.Fill };
                    list.Items.AddRange(titles);
                    list.SelectedIndex = 0;

                    var btn = new Button { Text = "Export", Dock = DockStyle.Bottom };
                    btn.Click += (s, ev) => picker.DialogResult = DialogResult.OK;

                    picker.Controls.Add(list);
                    picker.Controls.Add(btn);

                    if (picker.ShowDialog() != DialogResult.OK) return;

                    var selected = movies[list.SelectedIndex];
                    content = MovieToText(selected);
                }
                else
                {
                    //Get all people and let user pick by name
                    var people = await Supabase.GetAll<Person>(
                        "people?select=id,name,info,profile_path,date_of_birth,gender,place_of_birth"
                    );

                    var names = people.Select(p => p.name).ToArray();
                    if (names.Length == 0) { MessageBox.Show("No people found."); return; }

                    var picker = new Form();
                    picker.Text = "Select Person";
                    picker.Width = 350;
                    picker.Height = 200;
                    picker.StartPosition = FormStartPosition.CenterParent;

                    var list = new ListBox { Dock = DockStyle.Fill };
                    list.Items.AddRange(names);
                    list.SelectedIndex = 0;

                    var btn = new Button { Text = "Export", Dock = DockStyle.Bottom };
                    btn.Click += (s, ev) => picker.DialogResult = DialogResult.OK;

                    picker.Controls.Add(list);
                    picker.Controls.Add(btn);

                    if (picker.ShowDialog() != DialogResult.OK) return;

                    var selected = people[list.SelectedIndex];
                    content = PersonToText(selected);
                }

                //Let user choose save location and file name
                using SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save export file";
                sfd.Filter = "Text files (*.txt)|*.txt";
                sfd.DefaultExt = "txt";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                File.WriteAllText(sfd.FileName, content);
                MessageBox.Show("Exported successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}");
            }
        }
    }
}