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
        public string poster_path { get; set; }
        public string status { get; set; }
        public bool adult { get; set; }
        public string director { get; set; }
        public List<MovieGenre> movie_genres { get; set; }
    }
    public class MovieGenre
    {
        public Genre genres { get; set; } // nested genre object
    }

    public class Genre
    {
        public string name { get; set; }
    }
}
