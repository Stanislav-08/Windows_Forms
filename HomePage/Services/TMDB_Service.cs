using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Services
{
    public class TMDB_Service
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private readonly string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNiaGx6eWNoa3Nkc21odGF3aHJwIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc3NjI1NTMwNiwiZXhwIjoyMDkxODMxMzA2fQ.EKvHTSBWZZr33LwTPH9p_dE7dVys8zxEuaw6bQOMz7o";
        private readonly string tmdbKey = "682693c99aa733b5c721b59c841f9748";

        public TMDB_Service()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("apikey", apiKey);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
            client.DefaultRequestHeaders.Add("Prefer", "return=minimal");
        }

        public async Task Sync()
        {
            var movies = await GetTmdbMovies();

            foreach (var m in movies)
            {
                if (string.IsNullOrEmpty(m.poster_path))
                    continue;

                try
                {
                    var bytes = await DownloadImage(m.poster_path);

                    await UploadImage(bytes, m.id);

                    await InsertMovie(m);

                    await Task.Delay(50); // throttle
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async Task<List<TmdbMovie>> GetTmdbMovies()
        {
            string url =
                $"https://api.themoviedb.org/3/movie/popular?api_key={tmdbKey}";

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

             MessageBox.Show(json); // DEBUG

            if (!response.IsSuccessStatusCode)
                return new List<TmdbMovie>();

            var data = JsonSerializer.Deserialize<TmdbResponse>(json);

            return data?.results ?? new List<TmdbMovie>();
        }

        private async Task<byte[]> DownloadImage(string posterPath)
        {
            string url = "https://image.tmdb.org/t/p/w500" + posterPath;
            return await client.GetByteArrayAsync(url);
        }

        private async Task UploadImage(byte[] bytes, int tmdbId)
        {
            string path = $"posters/{tmdbId}.jpg";

            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType =
                new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{supabaseUrl}/storage/v1/object/movies/{path}"
            );

            request.Content = content;
            request.Headers.Add("apikey", apiKey);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                MessageBox.Show("UPLOAD FAILED:\n" + result);
        }

        private async Task InsertMovie(TmdbMovie m)
        {
            var payload = new
            {
                title = m.title,
                poster_path = $"posters/{m.id}.jpg",
                description = "",
                rating = 0,
                release_year = 0,
                duration_minutes = 0
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{supabaseUrl}/rest/v1/movies"
            );

            request.Content = content;
            request.Headers.Add("apikey", apiKey);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                MessageBox.Show("INSERT FAILED:\n" + result);
        }
    }

    public class TmdbResponse
    {
        public List<TmdbMovie> results { get; set; }
    }

    public class TmdbMovie
    {
        public int id { get; set; }
        public string title { get; set; }
        public string poster_path { get; set; }
    }
}