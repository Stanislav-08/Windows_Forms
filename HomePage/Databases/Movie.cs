using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Databases
{
    public class Movie
    {
        public string title { get; set; }
        public int duration_minutes { get; set; }
        public double rating { get; set; }
        public int release_year { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }
}
