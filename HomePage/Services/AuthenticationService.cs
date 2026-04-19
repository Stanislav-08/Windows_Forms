using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Services
{
    public class AuthenticationService
    {
        public readonly HttpClient client = new HttpClient();
        public readonly string supabaseUrl = "https://sbhlzychksdsmhtawhrp.supabase.co";
        public readonly string apiKey = "sb_publishable_xFOf5KlGK5vMTPLz1cdH6Q_Q14a5n5f";

        public AuthenticationService()
        {
            client.DefaultRequestHeaders.Add("apikey", apiKey);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        // Registration method
        public async Task<bool> Register(string email, string password, string displayName, string dateOfBirth)
        {
            var payload = new
            {
                email,
                password,
                data = new
                {
                    display_name = displayName,
                    date_of_birth = dateOfBirth
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                $"{supabaseUrl}/auth/v1/signup",
                content
            );

            var json = await response.Content.ReadAsStringAsync();

            // DEBUG OUTPUT 
            MessageBox.Show(json);

            return response.IsSuccessStatusCode;
        }

        //Login method
        public async Task<string?> Login(string email, string password)
        {
            var payload = new
            {
                email,
                password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(
                $"{supabaseUrl}/auth/v1/token?grant_type=password",
                content
            );

            var json = await response.Content.ReadAsStringAsync();

            // DEBUG OUTPUT
            MessageBox.Show(json);

            if (!response.IsSuccessStatusCode)
                return null;

            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

        //Data extraction method
        public async Task<List<T>> GetDatabase<T>(string database)
        {
            var response = await client.GetAsync($"{supabaseUrl}/rest/v1/{database}?select=*");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
