using System;
using System.Collections.Generic;
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
        private readonly string apiKey = "YOUR_API_KEY";
        private readonly string tmdbKey = "YOUR_TMDB_KEY";

        public TMDB_Service()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("apikey", apiKey);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
            client.DefaultRequestHeaders.Add("Prefer", "return=minimal");
        }

        public async Task SyncAll()
        {
            // MessageBox.Show("SYNCALL START");

            await SyncMovies();

            // MessageBox.Show("SYNC MOVIES DONE");

            await SyncPeople();

            // MessageBox.Show("SYNC PEOPLE DONE");
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
                    var bytes = await DownloadImage(m.poster_path);

                    await UploadImage(bytes, $"movies/{m.id}.jpg");

                    await InsertMovie(m);

                    await Task.Delay(50);
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                }
            }
        }

        private async Task<List<TmdbMovie>> GetTmdbMovies()
        {
            string url =
                $"https://api.themoviedb.org/3/movie/popular?api_key={tmdbKey}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<TmdbMovie>();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TmdbMovieResponse>(json);

            return data?.results ?? new List<TmdbMovie>();
        }

        // ===================== PEOPLE =====================

        public async Task SyncPeople()
        {
            // MessageBox.Show("SYNC PEOPLE START");

            var people = await GetTmdbPeople();

            // MessageBox.Show("PEOPLE COUNT: " + people.Count);

            foreach (var p in people)
            {
                // MessageBox.Show("PERSON: " + p.name);

                if (string.IsNullOrEmpty(p.profile_path))
                    continue;

                try
                {
                    var bytes = await DownloadImage(p.profile_path);

                    await UploadImage(bytes, $"people/{p.id}.jpg");

                    await InsertPerson(p);

                    await Task.Delay(50);
                }
                catch (Exception ex)
                {
                    // MessageBox.Show("PERSON ERROR: " + ex.Message);
                }
            }

            // MessageBox.Show("SYNC PEOPLE END");
        }

        private async Task<List<TmdbPerson>> GetTmdbPeople()
        {
            string url =
                $"https://api.themoviedb.org/3/person/popular?api_key={tmdbKey}";

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
            content.Headers.ContentType =
                new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{supabaseUrl}/storage/v1/object/pictures/{fullPath}"
            );

            request.Content = content;
            request.Headers.Add("apikey", apiKey);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                MessageBox.Show("UPLOAD FAILED: " + result);
        }

        // ===================== DATABASE =====================

        private async Task InsertMovie(TmdbMovie m)
        {
            var payload = new
            {
                title = m.title,
                poster_path = $"movies/{m.id}.jpg",
                description = "",
                rating = 0,
                release_year = 0,
                duration_minutes = 0
            };

            await InsertRow("movies", payload);
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
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                MessageBox.Show(table + " INSERT FAILED: " + result);
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

    public class TmdbPerson
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }
}