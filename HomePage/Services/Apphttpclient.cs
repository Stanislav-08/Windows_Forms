using System.Net.Http;

namespace App.Services
{
    public static class AppHttpClient
    {
        public static readonly HttpClient Instance;

        static AppHttpClient()
        {
            Instance = new HttpClient();
            Instance.DefaultRequestHeaders.Clear();
        }
    }
}