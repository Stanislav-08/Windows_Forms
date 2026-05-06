using App.Databases;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Services
{
    public class SupabaseClient
    {
        private static HttpClient Http = AppHttpClient.Instance;

        private string Url = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private string ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNiaGx6eWNoa3Nkc21odGF3aHJwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzYyNTUzMDYsImV4cCI6MjA5MTgzMTMwNn0.OMvyoMtnfKwq33C9Lm8PiwpuZvGx9S2av9CatqiXOvU";

        private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //----------REST — READ----------

        public async Task<List<T>> GetAll<T>(string table, int maxRows = 10000)
        {
            string url = table.Contains('?')
                ? $"{Url}/rest/v1/{table}"
                : $"{Url}/rest/v1/{table}?select=*";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddApiHeaders(request);
            request.Headers.TryAddWithoutValidation("Range", $"0-{maxRows - 1}");
            request.Headers.TryAddWithoutValidation("Range-Unit", "items");
            request.Headers.TryAddWithoutValidation("Prefer", "count=none");

            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"GET {table} FAILED ({(int)response.StatusCode}): {error}");
                return new List<T>();
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DESERIALIZE {table} FAILED: {ex.Message}\n\nJSON:\n{json[..Math.Min(json.Length, 300)]}");
                return new List<T>();
            }
        }

        public async Task<List<T>> Query<T>(string table, string filter, string select = "id")
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{Url}/rest/v1/{table}?{filter}&select={select}");
            AddApiHeaders(request);

            var response = await Http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
            }
            catch 
            {
                return new List<T>(); 
            }
        }

        //----------REST — WRITE----------

        public async Task<long?> InsertAndReturnId(string table, object payload)
        {
            var request = BuildRequest(HttpMethod.Post, $"/rest/v1/{table}", payload, prefer: "return=representation");
            var response = await Http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"{table} INSERT FAILED ({(int)response.StatusCode}): {json}");
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

        public async Task Insert(string table, object payload)
        {
            var request = BuildRequest(HttpMethod.Post, $"/rest/v1/{table}", payload);
            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"{table} INSERT FAILED ({(int)response.StatusCode}): {await response.Content.ReadAsStringAsync()}");
            }
        }

        //----------Watchlist addition function----------
        public async Task<bool> AddToWatchlist(long movieId, string userToken)
        {
            var request = BuildRequest(
                HttpMethod.Post,
                "/rest/v1/watchlist",
                new { movie_id = movieId },
                prefer: "return=minimal",
                userToken: userToken
                );

            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"AddToWatchlist FAILED ({(int)response.StatusCode}): {await response.Content.ReadAsStringAsync()}");
                return false;
            }

            return true;
        }

        //----------Watchlist removal function----------

        public async Task<bool> RemoveFromWatchlist(long movieId, string userToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Url}/rest/v1/watchlist?movie_id=eq.{movieId}");
            AddApiHeaders(request, userToken: userToken);

            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"RemoveFromWatchlist FAILED ({(int)response.StatusCode}): {await response.Content.ReadAsStringAsync()}");
                return false;
            }

            return true;
        }

        //----------Get watchlist function----------

        public async Task<List<WatchlistMovie>> GetWatchlist(string userToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Url}/rest/v1/watchlist?select=movie_id," +
                $"movies(id,title,poster_path,rating,release_year,duration_minutes,description,status,adult,director," +
                $"movie_genres(genres(name)),movie_people(role,people(name,profile_path)))");
            AddApiHeaders(request, userToken: userToken);

            var response = await Http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<List<WatchlistMovie>>(json, JsonOptions) ?? new List<WatchlistMovie>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"GetWatchlist FAILED: {ex.Message}");
                return new List<WatchlistMovie>();
            }
        }

        //----------Supabase storage function----------

        public async Task UploadImage(byte[] bytes, string storagePath)
        {
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Url}/storage/v1/object/pictures/{storagePath}");
            request.Content = content;
            AddApiHeaders(request);

            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"UPLOAD FAILED ({(int)response.StatusCode}): {await response.Content.ReadAsStringAsync()}");
            }
        }

        //----------Get single movie by id----------

        public async Task<Movies?> GetMovieById(long id)
        {
            var results = await GetAll<Movies>(
                $"movies?select=id,title,poster_path,duration_minutes,rating,release_year,description,status,adult,director,movie_genres(genres(name)),movie_people(role,people(name,profile_path))&id=eq.{id}"
            );
            return results.Count > 0 ? results[0] : null;
        }

        //----------Get single person by id----------

        public async Task<Person?> GetPersonById(long id)
        {
            var results = await GetAll<Person>(
                $"people?select=*,movie_people(role,movies(id,title,poster_path))&id=eq.{id}"
            );
            return results.Count > 0 ? results[0] : null;
        }

        //----------Helpers----------

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, object body, string? prefer = null, string? userToken = null)
        {
            var request = new HttpRequestMessage(method, $"{Url}{path}");
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            AddApiHeaders(request, prefer, userToken);
            return request;
        }

        private void AddApiHeaders(HttpRequestMessage request, string? prefer = null, string? userToken = null)
        {
            request.Headers.Remove("apikey");
            request.Headers.Remove("Authorization");
            request.Headers.Remove("Prefer");

            request.Headers.TryAddWithoutValidation("apikey", ApiKey);
            string token = userToken ?? ApiKey;
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            if (!string.IsNullOrEmpty(prefer))
            {
                request.Headers.TryAddWithoutValidation("Prefer", prefer);
            }
        }
    }

    public class WatchlistMovie
    {
        public long movie_id { get; set; }
        public Movies movies { get; set; }
    }
}