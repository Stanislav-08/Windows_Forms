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
        // Use the single shared client — no DefaultRequestHeaders ever touched here
        private static readonly HttpClient client = AppHttpClient.Instance;

        private readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private readonly string apiKey = "sb_publishable_xFOf5KlGK5vMTPLz1cdH6Q_Q14a5n5f";
        private readonly string tmdbKey = "682693c99aa733b5c721b59c841f9748";

        // -------------------------------------------------------------------
        // Public entry-point — only called on first launch (guard is in MainPage)
        // -------------------------------------------------------------------
        public async Task SyncAll()
        {
            await SyncMovies();
            await SyncPeople();
        }

        // ===================================================================
        // MOVIES
        // ===================================================================

        public async Task SyncMovies()
        {
            for (int page = 1; page <= 5; page++)
            {
                var movies = await GetTmdbMovies(page);
                await Task.Delay(500);

                foreach (var m in movies)
                {
                    if (string.IsNullOrEmpty(m.poster_path))
                        continue;

                    try
                    {
                        var details = await GetMovieDetails(m.id);
                        var director = await GetDirector(m.id);

                        var bytes = await client.GetByteArrayAsync("https://image.tmdb.org/t/p/w500" + m.poster_path);
                        await UploadImage(bytes, $"movies/{m.id}.jpg");

                        await InsertMovie(m, details, director);

                        await Task.Delay(300);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"MOVIE ERROR (id={m.id}): {ex.Message}");
                    }
                }
            }
        }

        private async Task<List<TmdbMovie>> GetTmdbMovies(int page)
        {
            // TMDB calls need no auth headers — just a plain GET
            var response = await client.GetAsync(
                $"https://api.themoviedb.org/3/movie/popular?api_key={tmdbKey}&page={page}"
            );

            if (!response.IsSuccessStatusCode) return new List<TmdbMovie>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieResponse>(json)?.results ?? new List<TmdbMovie>();
        }

        private async Task<TmdbMovieDetails> GetMovieDetails(int id)
        {
            var response = await client.GetAsync(
                $"https://api.themoviedb.org/3/movie/{id}?api_key={tmdbKey}"
            );

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieDetails>(json);
        }

        private async Task<string> GetDirector(int id)
        {
            var json = await client.GetStringAsync($"https://api.themoviedb.org/3/movie/{id}/credits?api_key={tmdbKey}");
            var data = JsonSerializer.Deserialize<TmdbCredits>(json);
            var director = data?.crew?.FirstOrDefault(c => c.job == "Director");
            return director?.name ?? "";
        }

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
                director = director,
            };

            var movieId = await InsertAndReturnId("movies", payload);

            if (movieId == null || d?.genres == null) return;

            foreach (var g in d.genres)
            {
                var genreId = await GetOrCreateGenre(g.name);
                await InsertRow("movie_genres", new { movie_id = movieId.Value, genre_id = genreId });
            }
        }

        // ===================================================================
        // PEOPLE
        // ===================================================================

        public async Task SyncPeople()
        {
            var people = await GetTmdbPeople();

            foreach (var p in people)
            {
                if (string.IsNullOrEmpty(p.profile_path)) continue;

                try
                {
                    var bytes = await client.GetByteArrayAsync("https://image.tmdb.org/t/p/w500" + p.profile_path);
                    await UploadImage(bytes, $"people/{p.id}.jpg");
                    await InsertRow("people", new
                    {
                        name = p.name,
                        profile_path = $"people/{p.id}.jpg",
                        info = ""
                    });
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PERSON ERROR (id={p.id}): {ex.Message}");
                }
            }
        }

        private async Task<List<TmdbPerson>> GetTmdbPeople()
        {
            var response = await client.GetAsync(
                $"https://api.themoviedb.org/3/person/popular?api_key={tmdbKey}"
            );

            if (!response.IsSuccessStatusCode) return new List<TmdbPerson>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbPersonResponse>(json)?.results ?? new List<TmdbPerson>();
        }

        // ===================================================================
        // STORAGE
        // ===================================================================

        private async Task UploadImage(byte[] bytes, string fullPath)
        {
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"{supabaseUrl}/storage/v1/object/pictures/{fullPath}"
            );
            request.Content = content;
            AddSupabaseHeaders(request);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                MessageBox.Show("UPLOAD FAILED: " + await response.Content.ReadAsStringAsync());
        }

        // ===================================================================
        // SUPABASE HELPERS
        // ===================================================================

        // Insert a row and return the new id, or null on failure
        private async Task<long?> InsertAndReturnId(string table, object payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{supabaseUrl}/rest/v1/{table}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            AddSupabaseHeaders(request, prefer: "return=representation");

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"{table} INSERT FAILED: " + json);
                return null;
            }

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                    return root[0].GetProperty("id").GetInt64();
            }
            catch { }

            return null;
        }

        // Insert a row without needing the returned id
        private async Task InsertRow(string table, object payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{supabaseUrl}/rest/v1/{table}");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            AddSupabaseHeaders(request);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"{table} INSERT FAILED: " + await response.Content.ReadAsStringAsync());
        }

        // Get existing genre id, or create it and return the new id
        private async Task<long> GetOrCreateGenre(string name)
        {
            var encoded = Uri.EscapeDataString(name);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{supabaseUrl}/rest/v1/genres?name=eq.{encoded}&select=id");
            AddSupabaseHeaders(request);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                    return doc.RootElement[0].GetProperty("id").GetInt64();
            }
            catch { }

            // Not found — insert it
            var newId = await InsertAndReturnId("genres", new { name });
            return newId ?? 0;
        }

        // Stamp Supabase auth headers onto a request — always per-request, never global
        private void AddSupabaseHeaders(HttpRequestMessage request, string prefer = null)
        {
            request.Headers.TryAddWithoutValidation("apikey", apiKey);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {apiKey}");

            if (!string.IsNullOrEmpty(prefer))
                request.Headers.TryAddWithoutValidation("Prefer", prefer);
        }

        private int ExtractYear(string date)
        {
            if (string.IsNullOrEmpty(date)) return 0;
            if (DateTime.TryParse(date, out var dt)) return dt.Year;
            return 0;
        }
    }

    // ===================================================================
    // MODELS
    // ===================================================================

    public class TmdbMovieResponse { public List<TmdbMovie> results { get; set; } }
    public class TmdbPersonResponse { public List<TmdbPerson> results { get; set; } }

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
        public List<Genre> genres { get; set; }
    }

    public class Genre { public string name { get; set; } }

    public class TmdbCredits { public List<Crew> crew { get; set; } }

    public class Crew { public string job { get; set; } public string name { get; set; } }

    public class TmdbPerson
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }
}