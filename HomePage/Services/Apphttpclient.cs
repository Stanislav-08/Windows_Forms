using System.Net.Http;

namespace App.Services
{
    public static class AppHttpClient
    {
        public static HttpClient Instance;

        static AppHttpClient()
        {
            Instance = new HttpClient();
            Instance.DefaultRequestHeaders.Clear();
        }
    }
}