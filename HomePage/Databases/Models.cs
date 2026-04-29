using System.Collections.Generic;

namespace App.Databases
{
    public class Models
    {
        public long id { get; set; }
        public string title { get; set; }
        public int duration_minutes { get; set; }
        public double rating { get; set; }
        public int release_year { get; set; }
        public string description { get; set; }
        public string poster_path { get; set; }
        public string status { get; set; }
        public bool adult { get; set; }
        public string director { get; set; }
        public List<MovieGenre>? movie_genres { get; set; }
        public List<MoviePerson>? movie_people { get; set; }
    }

    public class MovieGenre
    {
        public long movie_id { get; set; }
        public long genre_id { get; set; }
        public Genre genres { get; set; }
    }

    public class Genre
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class MoviePerson
    {
        public long movie_id { get; set; }
        public long person_id { get; set; }
        public string role { get; set; }
        public Person people { get; set; }
    }

    public class Person
    {
        public long id { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string profile_path { get; set; }
    }
}