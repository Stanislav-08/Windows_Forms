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
    public class SupabaseClient
    {
        private static readonly HttpClient Http = AppHttpClient.Instance;

        private readonly string Url = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private readonly string ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNiaGx6eWNoa3Nkc21odGF3aHJwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzYyNTUzMDYsImV4cCI6MjA5MTgzMTMwNn0.OMvyoMtnfKwq33C9Lm8PiwpuZvGx9S2av9CatqiXOvU";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // ===================================================================
        // AUTH
        // ===================================================================

        public async Task<bool> Register(string email, string password, string displayName, string dateOfBirth)
        {
            var payload = new
            {
                email,
                password,
                data = new { display_name = displayName, date_of_birth = dateOfBirth }
            };

            var request = BuildRequest(HttpMethod.Post, "/auth/v1/signup", payload);
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> Login(string email, string password)
        {
            var payload = new { email, password };

            var request = BuildRequest(HttpMethod.Post, "/auth/v1/token?grant_type=password", payload);
            var response = await Http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            try
            {
                var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("access_token").GetString();
            }
            catch
            {
                return null;
            }
        }

        public async Task Logout(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Url}/auth/v1/logout");
            AddApiHeaders(request);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
            await Http.SendAsync(request);
        }

        // ===================================================================
        // REST — READ
        // ===================================================================

        /// <summary>
        /// Pass just the table name (e.g. "movies") for select=*,
        /// OR a full query string (e.g. "movies?select=*,movie_genres(genres(name))")
        /// and this method will NOT append its own ?select=*.
        /// </summary>
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

        // ===================================================================
        // REST — WRITE
        // ===================================================================

        /// <summary>Inserts a row and returns the new auto-generated id, or null on failure.</summary>
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

        /// <summary>Inserts a row; ignores the response body.</summary>
        public async Task Insert(string table, object payload)
        {
            var request = BuildRequest(HttpMethod.Post, $"/rest/v1/{table}", payload);
            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"{table} INSERT FAILED ({(int)response.StatusCode}): {await response.Content.ReadAsStringAsync()}");
        }

        // ===================================================================
        // STORAGE
        // ===================================================================

        public async Task UploadImage(byte[] bytes, string storagePath)
        {
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{Url}/storage/v1/object/pictures/{storagePath}");
            request.Content = content;
            AddApiHeaders(request);

            var response = await Http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"UPLOAD FAILED ({(int)response.StatusCode}): {await response.Content.ReadAsStringAsync()}");
        }

        // ===================================================================
        // PRIVATE HELPERS
        // ===================================================================

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, object body, string? prefer = null)
        {
            var request = new HttpRequestMessage(method, $"{Url}{path}");
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            AddApiHeaders(request, prefer);
            return request;
        }

        private void AddApiHeaders(HttpRequestMessage request, string? prefer = null)
        {
            request.Headers.Remove("apikey");
            request.Headers.Remove("Authorization");
            request.Headers.Remove("Prefer");

            request.Headers.TryAddWithoutValidation("apikey", ApiKey);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {ApiKey}");

            if (!string.IsNullOrEmpty(prefer))
                request.Headers.TryAddWithoutValidation("Prefer", prefer);
        }
    }
}