using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Services
{
    public class TMDB_Service
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private readonly string apiKey = "sb_publishable_xFOf5KlGK5vMTPLz1cdH6Q_Q14a5n5f";
        private readonly string tmdbKey = "682693c99aa733b5c721b59c841f9748";

        public TMDB_Service()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("apikey", apiKey);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task SyncAll()
        {
            await SyncMovies();
            await SyncPeople();
        }

        // ===================== MOVIES =====================

        public async Task SyncMovies()
        {
            var movies = await GetTmdbMovies();

            foreach (var m in movies)
            {
                if (string.IsNullOrEmpty(m.poster_path))
                    continue;

                try
                {
                    var details = await GetMovieDetails(m.id);
                    var director = await GetDirector(m.id);

                    var bytes = await DownloadImage(m.poster_path);
                    await UploadImage(bytes, $"movies/{m.id}.jpg");

                    await InsertMovie(m, details, director);

                    await Task.Delay(100); // avoid rate limits
                }
                catch
                {
                    // ignore broken entries
                }
            }
        }

        private async Task<List<TmdbMovie>> GetTmdbMovies()
        {
            string url = $"https://api.themoviedb.org/3/movie/popular?api_key={tmdbKey}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<TmdbMovie>();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TmdbMovieResponse>(json);

            return data?.results ?? new List<TmdbMovie>();
        }

        private async Task<TmdbMovieDetails> GetMovieDetails(int id)
        {
            string url = $"https://api.themoviedb.org/3/movie/{id}?api_key={tmdbKey}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieDetails>(json);
        }

        private async Task<string> GetDirector(int id)
        {
            string url = $"https://api.themoviedb.org/3/movie/{id}/credits?api_key={tmdbKey}";
            var json = await client.GetStringAsync(url);

            var data = JsonSerializer.Deserialize<TmdbCredits>(json);
            var director = data?.crew?.FirstOrDefault(c => c.job == "Director");

            return director?.name ?? "";
        }

        // ===================== PEOPLE =====================

        public async Task SyncPeople()
        {
            var people = await GetTmdbPeople();

            foreach (var p in people)
            {
                if (string.IsNullOrEmpty(p.profile_path))
                    continue;

                try
                {
                    var bytes = await DownloadImage(p.profile_path);
                    await UploadImage(bytes, $"people/{p.id}.jpg");

                    await InsertPerson(p);

                    await Task.Delay(100);
                }
                catch { }
            }
        }

        private async Task<List<TmdbPerson>> GetTmdbPeople()
        {
            string url = $"https://api.themoviedb.org/3/person/popular?api_key={tmdbKey}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<TmdbPerson>();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TmdbPersonResponse>(json);

            return data?.results ?? new List<TmdbPerson>();
        }

        // ===================== STORAGE =====================

        private async Task<byte[]> DownloadImage(string path)
        {
            string url = "https://image.tmdb.org/t/p/w500" + path;
            return await client.GetByteArrayAsync(url);
        }

        private async Task UploadImage(byte[] bytes, string fullPath)
        {
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{supabaseUrl}/storage/v1/object/pictures/{fullPath}"
            );

            request.Content = content;
            request.Headers.Add("apikey", apiKey);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                MessageBox.Show("UPLOAD FAILED: " + result);
            }
        }

        // ===================== DATABASE =====================

        private async Task InsertMovie(TmdbMovie m, TmdbMovieDetails d, string director)
        {
            var payload = new
            {
                title = m.title,
                poster_path = $"movies/{m.id}.jpg",
                description = d?.overview ?? "",
                rating = d?.vote_average ?? 0,
                release_year = ExtractYear(d?.release_date),
                duration_minutes = d?.runtime ?? 0,
                status = d?.status,
                adult = d?.adult,
                genres = d?.genres?.Select(g => g.name).ToList(),
                director = director,
                imdb_id = d?.imdb_id
            };

            await InsertRow("movies", payload);
        }

        private int ExtractYear(string date)
        {
            if (string.IsNullOrEmpty(date)) return 0;
            if (DateTime.TryParse(date, out var dt))
                return dt.Year;
            return 0;
        }

        private async Task InsertPerson(TmdbPerson p)
        {
            var payload = new
            {
                name = p.name,
                profile_path = $"people/{p.id}.jpg",
                info = ""
            };

            await InsertRow("people", payload);
        }

        private async Task InsertRow(string table, object payload)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{supabaseUrl}/rest/v1/{table}"
            );

            request.Content = content;
            request.Headers.Add("apikey", apiKey);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                MessageBox.Show(table + " INSERT FAILED: " + result);
            }
        }
    }

    // ===================== MODELS =====================

    public class TmdbMovieResponse
    {
        public List<TmdbMovie> results { get; set; }
    }

    public class TmdbPersonResponse
    {
        public List<TmdbPerson> results { get; set; }
    }

    public class TmdbMovie
    {
        public int id { get; set; }
        public string title { get; set; }
        public string poster_path { get; set; }
    }

    public class TmdbMovieDetails
    {
        public string overview { get; set; }
        public bool adult { get; set; }
        public string status { get; set; }
        public int runtime { get; set; }
        public double vote_average { get; set; }
        public string release_date { get; set; }
        public string imdb_id { get; set; }
        public List<Genre> genres { get; set; }
    }

    public class Genre
    {
        public string name { get; set; }
    }

    public class TmdbCredits
    {
        public List<Crew> crew { get; set; }
    }

    public class Crew
    {
        public string job { get; set; }
        public string name { get; set; }
    }

    public class TmdbPerson
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }
}