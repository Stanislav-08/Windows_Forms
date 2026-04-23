using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Services
{
    public class AuthenticationService
    {
        // Use the single shared client — no DefaultRequestHeaders ever touched here
        private static readonly HttpClient client = AppHttpClient.Instance;

        public readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";
        public readonly string apiKey = "sb_publishable_xFOf5KlGK5vMTPLz1cdH6Q_Q14a5n5f";

        // -------------------------------------------------------------------
        // Register a new user via Supabase Auth
        // -------------------------------------------------------------------
        public async Task<bool> Register(string email, string password, string displayName, string dateOfBirth)
        {
            var payload = new
            {
                email,
                password,
                data = new { display_name = displayName, date_of_birth = dateOfBirth }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{supabaseUrl}/auth/v1/signup");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            AddHeaders(request);

            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        // -------------------------------------------------------------------
        // Log in — returns the access token, or null on failure
        // -------------------------------------------------------------------
        public async Task<string?> Login(string email, string password)
        {
            var payload = new { email, password };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{supabaseUrl}/auth/v1/token?grant_type=password");
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            AddHeaders(request);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

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

        // -------------------------------------------------------------------
        // Fetch all rows from any Supabase table.
        // Uses per-request Range header so Supabase returns more than the
        // default page size (which was causing only 2 rows to load).
        // -------------------------------------------------------------------
        public async Task<List<T>> GetDatabase<T>(string table, int maxRows = 10000)
        {
            // Build a fresh request every time — no shared header state
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{supabaseUrl}/rest/v1/{table}?select=*"
            );

            AddHeaders(request);

            // Ask Supabase for up to maxRows rows (inclusive range)
            request.Headers.TryAddWithoutValidation("Range", $"0-{maxRows - 1}");
            request.Headers.TryAddWithoutValidation("Range-Unit", "items");
            request.Headers.TryAddWithoutValidation("Prefer", "count=none");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"GET {table} FAILED: {error}");
                return new List<T>();
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DESERIALIZE {table} FAILED: {ex.Message}");
                return new List<T>();
            }
        }

        // Stamp Supabase auth headers onto a request — always per-request, never global
        private void AddHeaders(HttpRequestMessage request)
        {
            request.Headers.TryAddWithoutValidation("apikey", apiKey);
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {apiKey}");
        }
    }
}