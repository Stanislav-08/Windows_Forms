using System.Net.Http;

namespace App.Services
{
    public static class AppHttpClient
    {
        public static readonly HttpClient Instance = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };
    }
}