using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Services
{
    public class AuthenticationService
    {
        private static HttpClient Http = AppHttpClient.Instance;

        private string Url = "https://sbhlzychksdsmhtawhrp.supabase.co";
        private string ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNiaGx6eWNoa3Nkc21odGF3aHJwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzYyNTUzMDYsImV4cCI6MjA5MTgzMTMwNn0.OMvyoMtnfKwq33C9Lm8PiwpuZvGx9S2av9CatqiXOvU";

        private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        //----------Registration function----------

        public async Task<bool> Register(string email, string password, string displayName, string dateOfBirth)
        {
            var values = new
            {
                email,
                password,
                data = new { display_name = displayName, date_of_birth = dateOfBirth }
            };

            var request = BuildRequest(HttpMethod.Post, "/auth/v1/signup", values);
            var response = await Http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        //----------Login function----------

        public async Task<string?> Login(string email, string password)
        {
            var values = new { email, password };

            var request = BuildRequest(HttpMethod.Post, "/auth/v1/token?grant_type=password", values);
            var response = await Http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

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

        //----------Logout function----------

        public async Task Logout(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Url}/auth/v1/logout");
            AddHeaders(request, userToken: accessToken);
            await Http.SendAsync(request);
        }

        //----------Get username function----------

        public async Task<string?> GetDisplayName(string userToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,$"{Url}/rest/v1/users?select=display_name&limit=1");
            AddHeaders(request, userToken: userToken);

            var response = await Http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var rows = JsonSerializer.Deserialize<List<DisplayNameRow>>(json, JsonOptions);
                return rows?.FirstOrDefault()?.display_name;
            }
            catch
            {
                return null; 
            }
        }

        //----------Helpers----------

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, object body)
        {
            var request = new HttpRequestMessage(method, $"{Url}{path}");
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            AddHeaders(request);
            return request;
        }

        private void AddHeaders(HttpRequestMessage request, string? userToken = null)
        {
            request.Headers.Remove("apikey");
            request.Headers.Remove("Authorization");

            request.Headers.TryAddWithoutValidation("apikey", ApiKey);
            string token = userToken ?? ApiKey;
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
        }

        private class DisplayNameRow
        {
            public string display_name { get; set; }
        }
    }

    
}