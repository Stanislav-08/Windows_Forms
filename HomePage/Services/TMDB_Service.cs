using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Services
{
    public class TMDB_Service
    {
        private static readonly HttpClient Http = AppHttpClient.Instance;
        private readonly SupabaseClient Supabase;

        private const string TmdbBase = "https://api.themoviedb.org/3";
        private const string TmdbImage = "https://image.tmdb.org/t/p/w500";
        private readonly string _tmdbKey = "682693c99aa733b5c721b59c841f9748";

        public TMDB_Service(SupabaseClient supabase)
        {
            Supabase = supabase;
        }

        // ===================================================================
        // PUBLIC ENTRY-POINT
        // ===================================================================

        public async Task SyncAll()
        {
            await SyncMovies();
            await SyncPeople();
        }

        // ===================================================================
        // MOVIES
        // ===================================================================

        public async Task SyncMovies()
        {
            for (int page = 1; page <= 5; page++)
            {
                var movies = await FetchPopularMovies(page);
                await Task.Delay(500);

                foreach (var m in movies)
                {
                    if (string.IsNullOrEmpty(m.poster_path)) continue;

                    try
                    {
                        var details = await FetchMovieDetails(m.id);
                        var director = await FetchDirector(m.id);
                        var imageBytes = await Http.GetByteArrayAsync(TmdbImage + m.poster_path);

                        await Supabase.UploadImage(imageBytes, $"movies/{m.id}.jpg");
                        await InsertMovie(m, details, director);
                        await Task.Delay(300);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"MOVIE ERROR (id={m.id}): {ex.Message}");
                    }
                }
            }
        }

        private async Task<List<TmdbMovie>> FetchPopularMovies(int page)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/popular?api_key={_tmdbKey}&page={page}");
            if (!response.IsSuccessStatusCode) return new List<TmdbMovie>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieResponse>(json)?.results ?? new List<TmdbMovie>();
        }

        private async Task<TmdbMovieDetails?> FetchMovieDetails(int id)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/{id}?api_key={_tmdbKey}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieDetails>(json);
        }

        private async Task<string> FetchDirector(int id)
        {
            var json = await Http.GetStringAsync($"{TmdbBase}/movie/{id}/credits?api_key={_tmdbKey}");
            var credits = JsonSerializer.Deserialize<TmdbCredits>(json);
            return credits?.crew?.FirstOrDefault(c => c.job == "Director")?.name ?? "";
        }

        private async Task InsertMovie(TmdbMovie m, TmdbMovieDetails? d, string director)
        {
            var payload = new
            {
                title = m.title,
                poster_path = $"movies/{m.id}.jpg",
                description = d?.overview ?? "",
                rating = d?.vote_average ?? 0,
                release_year = ExtractYear(d?.release_date),
                duration_minutes = d?.runtime ?? 0,
                status = d?.status,
                adult = d?.adult,
                director
            };

            var movieId = await Supabase.InsertAndReturnId("movies", payload);

            if (movieId == null || d?.genres == null) return;

            foreach (var g in d.genres)
            {
                var genreId = await GetOrCreateGenre(g.name);
                await Supabase.Insert("movie_genres", new { movie_id = movieId.Value, genre_id = genreId });
            }
        }

        // ===================================================================
        // PEOPLE
        // ===================================================================

        public async Task SyncPeople()
        {
            var people = await FetchPopularPeople();

            foreach (var p in people)
            {
                if (string.IsNullOrEmpty(p.profile_path)) continue;

                try
                {
                    var imageBytes = await Http.GetByteArrayAsync(TmdbImage + p.profile_path);
                    await Supabase.UploadImage(imageBytes, $"people/{p.id}.jpg");
                    await Supabase.Insert("people", new
                    {
                        name = p.name,
                        profile_path = $"people/{p.id}.jpg",
                        info = ""
                    });
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PERSON ERROR (id={p.id}): {ex.Message}");
                }
            }
        }

        private async Task<List<TmdbPerson>> FetchPopularPeople()
        {
            var response = await Http.GetAsync($"{TmdbBase}/person/popular?api_key={_tmdbKey}");
            if (!response.IsSuccessStatusCode) return new List<TmdbPerson>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbPersonResponse>(json)?.results ?? new List<TmdbPerson>();
        }

        // ===================================================================
        // GENRE HELPER
        // ===================================================================

        private async Task<long> GetOrCreateGenre(string name)
        {
            var encoded = Uri.EscapeDataString(name);
            var existing = await Supabase.Query<GenreRow>("genres", $"name=eq.{encoded}");

            if (existing.Count > 0) return existing[0].id;

            return await Supabase.InsertAndReturnId("genres", new { name }) ?? 0;
        }

        // ===================================================================
        // UTILS
        // ===================================================================

        private static int ExtractYear(string? date)
        {
            if (string.IsNullOrEmpty(date)) return 0;
            return DateTime.TryParse(date, out var dt) ? dt.Year : 0;
        }
    }

    // ===================================================================
    // MODELS
    // ===================================================================

    file class GenreRow 
    {
        public long id { get; set; } 
    }

    public class TmdbMovieResponse 
    {
        public List<TmdbMovie> results { get; set; }
    }
    public class TmdbPersonResponse 
    {
        public List<TmdbPerson> results { get; set; }
    }

    public class TmdbMovie
    {
        public int id { get; set; }
        public string title { get; set; }
        public string poster_path { get; set; }
    }

    public class TmdbMovieDetails
    {
        public string overview { get; set; }
        public bool adult { get; set; }
        public string? status { get; set; }
        public int runtime { get; set; }
        public double vote_average { get; set; }
        public string? release_date { get; set; }
        public List<Genre> genres { get; set; }
    }

    public class Genre {
        public string name { get; set; }
    }

    public class TmdbCredits {
        public List<Crew> crew { get; set; }
    }
    public class Crew
    {
        public string job { get; set; }
        public string name { get; set; }
    }

    public class TmdbPerson
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }
}