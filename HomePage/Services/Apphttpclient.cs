using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public static class AppHttpClient
    {
        public static readonly HttpClient Instance = new HttpClient();
    }
}
