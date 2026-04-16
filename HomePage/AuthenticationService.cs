using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App
{

    /// <summary>
    /// VIJ GOOOOOOOOOOOOO
    /// </summary>


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
        public async Task<bool> Register(string email, string password, string displayName)
        {
            var payload = new
            {
                email = email,
                password = password,
                data = new
                {
                    display_name = displayName
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

        public async Task<string?> Login(string email, string password)
        {
            var payload = new
            {
                email = email,
                password = password
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
    }
}
