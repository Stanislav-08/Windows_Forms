using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
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

        // In-memory cache: person name -> supabase id
        private readonly Dictionary<string, long> _personCache = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        private readonly SemaphoreSlim _personCacheLock = new SemaphoreSlim(1, 1);

        public TMDB_Service(SupabaseClient supabase)
        {
            Supabase = supabase;
        }

        // ===================================================================
        // PUBLIC ENTRY-POINT
        // ===================================================================

        public async Task SyncAll()
        {
            await SyncPeople();       // people first
            await PreloadPersonCache(); // load all people into memory
            await SyncMovies();       // movies link to people via cache
        }

        // ===================================================================
        // PERSON CACHE
        // ===================================================================

        private async Task PreloadPersonCache()
        {
            var people = await Supabase.GetAll<PersonRow>("people?select=id,name");
            _personCache.Clear();
            foreach (var p in people)
                if (!string.IsNullOrEmpty(p.name))
                    _personCache[p.name] = p.id;

            MessageBox.Show($"Person cache loaded: {_personCache.Count} people");
        }

        // ===================================================================
        // MOVIES
        // ===================================================================

        public async Task SyncMovies()
        {
            int successCount = 0;
            int failCount = 0;

            for (int page = 1; page <= 5; page++)
            {
                List<TmdbMovie> movies;

                try
                {
                    movies = await FetchPopularMovies(page);
                    MessageBox.Show($"Page {page}: fetched {movies.Count} movies from TMDB");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to fetch page {page}: {ex.Message}");
                    break;
                }

                var tasks = movies
                    .Where(m => !string.IsNullOrEmpty(m.poster_path))
                    .Select(async m =>
                    {
                        try
                        {
                            var details = await FetchMovieDetails(m.id);
                            var credits = await FetchCredits(m.id);
                            var imageBytes = await Http.GetByteArrayAsync(TmdbImage + m.poster_path);

                            await Supabase.UploadImage(imageBytes, $"movies/{m.id}.jpg");
                            await InsertMovie(m, details, credits);

                            Interlocked.Increment(ref successCount);
                        }
                        catch (Exception ex)
                        {
                            Interlocked.Increment(ref failCount);
                            MessageBox.Show($"MOVIE ERROR (id={m.id}): {ex.Message}");
                        }
                    });

                await Task.WhenAll(tasks);
            }

            MessageBox.Show($"SyncMovies done: {successCount} succeeded, {failCount} failed");
        }

        private async Task<List<TmdbMovie>> FetchPopularMovies(int page)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/popular?api_key={_tmdbKey}&page={page}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"FetchPopularMovies FAILED ({(int)response.StatusCode})");
                return new List<TmdbMovie>();
            }
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

        private async Task<TmdbCredits?> FetchCredits(int id)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/{id}/credits?api_key={_tmdbKey}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbCredits>(json);
        }

        private async Task InsertMovie(TmdbMovie m, TmdbMovieDetails? d, TmdbCredits? credits)
        {
            string director = credits?.crew?.FirstOrDefault(c => c.job == "Director")?.name ?? "";

            var payload = new
            {
                title = m.title,
                poster_path = $"movies/{m.id}.jpg",
                description = d?.overview ?? "",
                rating = d?.vote_average ?? 0,
                release_year = ExtractYear(d?.release_date),
                duration_minutes = d?.runtime ?? 0,
                status = d?.status ?? "",
                adult = d?.adult ?? false,
                director
            };

            var movieId = await Supabase.InsertAndReturnId("movies", payload);
            if (movieId == null) return;

            // Genres
            if (d?.genres != null)
                foreach (var g in d.genres)
                {
                    var genreId = await GetOrCreateGenre(g.name);
                    await Supabase.Insert("movie_genres", new { movie_id = movieId.Value, genre_id = genreId });
                }

            // Top 10 cast only
            if (credits?.cast != null)
                foreach (var actor in credits.cast.Take(10))
                {
                    if (string.IsNullOrEmpty(actor.name)) continue;
                    var personId = await GetOrCreatePersonCached(actor.id, actor.name, actor.profile_path);
                    if (personId == null) continue;

                    string role = string.IsNullOrEmpty(actor.character)
                        ? "Actor"
                        : $"Actor ({actor.character})";

                    await Supabase.Insert("movie_people", new
                    {
                        movie_id = movieId.Value,
                        person_id = personId.Value,
                        role
                    });
                }
        }

        // ===================================================================
        // PEOPLE
        // ===================================================================

        public async Task SyncPeople()
        {
            int successCount = 0;
            int failCount = 0;

            for (int page = 1; page <= 3; page++)
            {
                List<TmdbPerson> people;

                try
                {
                    people = await FetchPopularPeople(page);
                    MessageBox.Show($"People page {page}: fetched {people.Count} people from TMDB");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to fetch people page {page}: {ex.Message}");
                    break;
                }

                // Fetch details for all people in parallel
                var tasks = people
                    .Where(p => !string.IsNullOrEmpty(p.profile_path))
                    .Select(async p =>
                    {
                        try
                        {
                            // Fetch full person details for bio fields
                            var details = await FetchPersonDetails(p.id);

                            var imageBytes = await Http.GetByteArrayAsync(TmdbImage + p.profile_path);
                            await Supabase.UploadImage(imageBytes, $"people/{p.id}.jpg");

                            await Supabase.Insert("people", new
                            {
                                name = p.name,
                                profile_path = $"people/{p.id}.jpg",
                                info = details?.biography ?? "",
                                date_of_birth = string.IsNullOrEmpty(details?.birthday) ? "1900-01-01" : details.birthday,
                                place_of_birth = details?.place_of_birth ?? "",
                                gender = ConvertGender(details?.gender)
                            });

                            Interlocked.Increment(ref successCount);
                        }
                        catch (Exception ex)
                        {
                            Interlocked.Increment(ref failCount);
                            MessageBox.Show($"PERSON ERROR (id={p.id}): {ex.Message}");
                        }
                    });

                await Task.WhenAll(tasks);
            }

            MessageBox.Show($"SyncPeople done: {successCount} succeeded, {failCount} failed");
        }

        private async Task<List<TmdbPerson>> FetchPopularPeople(int page = 1)
        {
            var response = await Http.GetAsync($"{TmdbBase}/person/popular?api_key={_tmdbKey}&page={page}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"FetchPopularPeople FAILED ({(int)response.StatusCode})");
                return new List<TmdbPerson>();
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbPersonResponse>(json)?.results ?? new List<TmdbPerson>();
        }

        private async Task<TmdbPersonDetails?> FetchPersonDetails(int id)
        {
            var response = await Http.GetAsync($"{TmdbBase}/person/{id}?api_key={_tmdbKey}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbPersonDetails>(json);
        }

        // TMDB gender: 0 = Not set, 1 = Female, 2 = Male, 3 = Non-binary
        private static string ConvertGender(int? gender) => gender switch
        {
            1 => "Female",
            2 => "Male",
            3 => "Non-binary",
            _ => "Unknown"
        };

        // ===================================================================
        // HELPERS
        // ===================================================================

        private async Task<long> GetOrCreateGenre(string name)
        {
            var encoded = Uri.EscapeDataString(name);
            var existing = await Supabase.Query<GenreRow>("genres", $"name=eq.{encoded}");
            if (existing.Count > 0) return existing[0].id;
            return await Supabase.InsertAndReturnId("genres", new { name }) ?? 0;
        }

        private async Task<long?> GetOrCreatePersonCached(int tmdbId, string name, string? tmdbProfilePath)
        {
            // 1. Check in-memory cache
            await _personCacheLock.WaitAsync();
            try
            {
                if (_personCache.TryGetValue(name, out long cachedId))
                    return cachedId;
            }
            finally
            {
                _personCacheLock.Release();
            }

            // 2. Not in cache — fetch details and insert
            var details = await FetchPersonDetails(tmdbId);

            string profilePath = "";
            if (!string.IsNullOrEmpty(tmdbProfilePath))
            {
                try
                {
                    var imageBytes = await Http.GetByteArrayAsync(TmdbImage + tmdbProfilePath);
                    profilePath = $"people/{tmdbId}.jpg";
                    await Supabase.UploadImage(imageBytes, profilePath);
                }
                catch
                {
                    profilePath = "";
                }
            }

            var newId = await Supabase.InsertAndReturnId("people", new
            {
                name,
                profile_path = profilePath,
                info = details?.biography ?? "",
                date_of_birth = string.IsNullOrEmpty(details?.birthday) ? "1900-01-01" : details.birthday,
                place_of_birth = details?.place_of_birth ?? "",
                gender = ConvertGender(details?.gender)
            });

            // 3. Add to cache
            if (newId.HasValue)
            {
                await _personCacheLock.WaitAsync();
                try { _personCache[name] = newId.Value; }
                finally { _personCacheLock.Release(); }
            }

            return newId;
        }

        private static int ExtractYear(string? date)
        {
            if (string.IsNullOrEmpty(date)) return 0;
            return DateTime.TryParse(date, out var dt) ? dt.Year : 0;
        }
    }

    // ===================================================================
    // INTERNAL QUERY MODELS
    // ===================================================================

    file class GenreRow { public long id { get; set; } }
    file class PersonRow { public long id { get; set; } public string name { get; set; } }

    // ===================================================================
    // TMDB API MODELS
    // ===================================================================

    public class TmdbMovieResponse { public List<TmdbMovie> results { get; set; } }
    public class TmdbPersonResponse { public List<TmdbPerson> results { get; set; } }

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

    public class Genre { public string name { get; set; } }

    public class TmdbCredits
    {
        public List<TmdbCastMember> cast { get; set; }
        public List<TmdbCrewMember> crew { get; set; }
    }

    public class TmdbCastMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? character { get; set; }
        public string? profile_path { get; set; }
    }

    public class TmdbCrewMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public string job { get; set; }
        public string? profile_path { get; set; }
    }

    public class TmdbPerson
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }

    public class TmdbPersonDetails
    {
        public string? biography { get; set; }
        public string? birthday { get; set; }
        public string? place_of_birth { get; set; }
        public int? gender { get; set; }
    }
}