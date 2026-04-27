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
        private static readonly HttpClient _http = AppHttpClient.Instance;

        private readonly string _url = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private readonly string _apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNiaGx6eWNoa3Nkc21odGF3aHJwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzYyNTUzMDYsImV4cCI6MjA5MTgzMTMwNn0.OMvyoMtnfKwq33C9Lm8PiwpuZvGx9S2av9CatqiXOvU";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // ===================================================================
        // AUTH
        // ===================================================================

        public async Task<string?> Register(string email, string password, string displayName, string dateOfBirth)
        {
            var payload = new
            {
                email,
                password,
                data = new { display_name = displayName, date_of_birth = dateOfBirth }
            };

            var request = BuildRequest(HttpMethod.Post, "/auth/v1/signup", payload);
            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            try
            {
                var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("access_token").GetString();
            }
            catch { return null; }
        }

        public async Task<string?> Login(string email, string password)
        {
            var payload = new { email, password };

            var request = BuildRequest(HttpMethod.Post, "/auth/v1/token?grant_type=password", payload);
            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            try
            {
                var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("access_token").GetString();
            }
            catch { return null; }
        }

        public async Task Logout(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/auth/v1/logout");
            request.Headers.TryAddWithoutValidation("apikey", _apiKey);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");
            await _http.SendAsync(request);
        }

        // ===================================================================
        // REST — READ
        // ===================================================================

        public async Task<List<T>> GetAll<T>(string tableAndQuery, int maxRows = 10000)
        {
            string url = tableAndQuery.Contains('?')
                ? $"{_url}/rest/v1/{tableAndQuery}&apikey={_apiKey}"
                : $"{_url}/rest/v1/{tableAndQuery}?select=*&apikey={_apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_apiKey}");
            request.Headers.TryAddWithoutValidation("Accept", "application/json");

            var response = await _http.SendAsync(request);

            // Read body ONCE
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"GET FAILED {response.StatusCode}: {json}");
                return new List<T>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DESERIALIZE FAILED: {ex.Message}\nJSON: {json.Substring(0, Math.Min(200, json.Length))}");
                return new List<T>();
            }
        }

        public async Task<List<T>> Query<T>(string table, string filter, string select = "id")
        {
            var url = $"{_url}/rest/v1/{table}?{filter}&select={select}&apikey={_apiKey}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_apiKey}");
            request.Headers.TryAddWithoutValidation("Accept", "application/json");

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        // ===================================================================
        // REST — WRITE
        // ===================================================================

        public async Task<long?> InsertAndReturnId(string table, object payload)
        {
            var request = BuildRequest(HttpMethod.Post, $"/rest/v1/{table}", payload, prefer: "return=representation");
            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"{table} INSERT FAILED: {json}");
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
            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"{table} INSERT FAILED: {json}");
        }

        // ===================================================================
        // STORAGE
        // ===================================================================

        public async Task UploadImage(byte[] bytes, string storagePath)
        {
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{_url}/storage/v1/object/pictures/{storagePath}");
            request.Content = content;
            AddApiHeaders(request);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                MessageBox.Show("UPLOAD FAILED: " + await response.Content.ReadAsStringAsync());
        }

        // ===================================================================
        // PRIVATE HELPERS
        // ===================================================================

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, object body, string? prefer = null)
        {
            var request = new HttpRequestMessage(method, $"{_url}{path}");
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            AddApiHeaders(request, prefer);
            return request;
        }

        private void AddApiHeaders(HttpRequestMessage request, string? prefer = null)
        {
            request.Headers.TryAddWithoutValidation("apikey", _apiKey);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_apiKey}");

            if (!string.IsNullOrEmpty(prefer))
                request.Headers.TryAddWithoutValidation("Prefer", prefer);
        }
    }
}