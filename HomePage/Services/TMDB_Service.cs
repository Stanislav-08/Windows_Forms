using System.Text.Json;

namespace App.Services
{
    public class TMDB_Service
    {
        private static HttpClient Http = AppHttpClient.Instance;
        private SupabaseClient Supabase;

        private string TmdbBase = "https://api.themoviedb.org/3";
        private string TmdbImage = "https://image.tmdb.org/t/p/w500";
        private string TmdbKey = "682693c99aa733b5c721b59c841f9748";

        //Person cache
        //Person name -> supabase id
        private Dictionary<string, long> PersonCache = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        private SemaphoreSlim PersonCacheLock = new SemaphoreSlim(1, 1);

        //Genre cache
        //Genre name -> supabase id
        private Dictionary<string, long> GenreCache = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        private SemaphoreSlim GenreCacheLock = new SemaphoreSlim(1, 1);

        public TMDB_Service(SupabaseClient supabase)
        {
            Supabase = supabase;
        }

        //----------Sync everything function----------
        public async Task SyncAll()
        {
            await SyncPeople();
            await PreloadPersonCache();
            await PreloadGenreCache();
            await SyncMovies();
        }

        //----------Preload person cache----------

        private async Task PreloadPersonCache()
        {
            var people = await Supabase.GetAll<PersonRow>("people?select=id,name");
            PersonCache.Clear();
            foreach (var p in people)
            {
                if (!string.IsNullOrEmpty(p.name))
                {
                    PersonCache[p.name] = p.id;
                }
            }
            MessageBox.Show($"Person cache loaded: {PersonCache.Count} people");
        }

        //----------Preload genre cache----------

        private async Task PreloadGenreCache()
        {
            var genres = await Supabase.GetAll<GenreRow>("genres?select=id,name");
            GenreCache.Clear();
            foreach (var g in genres)
            {
                if (!string.IsNullOrEmpty(g.name))
                {
                    GenreCache[g.name] = g.id;
                }
            }
            MessageBox.Show($"Genre cache loaded: {GenreCache.Count} genres");
        }

        //----------Sync movies----------

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

        //----------Fetch popular movies----------

        private async Task<List<TmdbMovie>> FetchPopularMovies(int page)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/popular?api_key={TmdbKey}&page={page}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"FetchPopularMovies FAILED ({(int)response.StatusCode})");
                return new List<TmdbMovie>();
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieResponse>(json)?.results ?? new List<TmdbMovie>();
        }

        //----------Fetch movie details----------

        private async Task<TmdbMovieDetails?> FetchMovieDetails(int id)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/{id}?api_key={TmdbKey}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieDetails>(json);
        }

        //----------Fetch movie credits----------

        private async Task<TmdbCredits?> FetchCredits(int id)
        {
            var response = await Http.GetAsync($"{TmdbBase}/movie/{id}/credits?api_key={TmdbKey}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbCredits>(json);
        }

        //----------Insert movie function----------

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

            if (d?.genres != null)
            {
                foreach (var g in d.genres)
                {
                    var genreId = await GetOrCreateGenreCached(g.name);
                    await Supabase.Insert("movie_genres", new { movie_id = movieId.Value, genre_id = genreId });
                }
            }

            if (credits?.cast != null)
            {
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
        }

        //----------Sync people----------

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

                var tasks = people
                    .Where(p => !string.IsNullOrEmpty(p.profile_path))
                    .Select(async p =>
                    {
                        try
                        {
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

        //----------Fetch popular people----------

        private async Task<List<TmdbPerson>> FetchPopularPeople(int page = 1)
        {
            var response = await Http.GetAsync($"{TmdbBase}/person/popular?api_key={TmdbKey}&page={page}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"FetchPopularPeople FAILED ({(int)response.StatusCode})");
                return new List<TmdbPerson>();
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbPersonResponse>(json)?.results ?? new List<TmdbPerson>();
        }

        //----------Fetch person details----------

        private async Task<TmdbPersonDetails?> FetchPersonDetails(int id)
        {
            var response = await Http.GetAsync($"{TmdbBase}/person/{id}?api_key={TmdbKey}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbPersonDetails>(json);
        }

        private static string ConvertGender(int? gender)
        {
            switch (gender)
            {
                case 1: return "Female";
                case 2: return "Male";
                case 3: return "Non-binary";
                default: return "Unknown";
            }
        }

        //----------Helpers----------

        private async Task<long> GetOrCreateGenreCached(string name)
        {
            await GenreCacheLock.WaitAsync();
            try
            {
                if (GenreCache.TryGetValue(name, out long cachedId))
                {
                    return cachedId;
                }
            }
            finally
            {
                GenreCacheLock.Release(); 
            }

            var newId = await Supabase.InsertAndReturnId("genres", new { name }) ?? 0;

            await GenreCacheLock.WaitAsync();
            try 
            {
                GenreCache[name] = newId;
            }
            finally 
            {
                GenreCacheLock.Release(); 
            }

            return newId;
        }

        private async Task<long?> GetOrCreatePersonCached(int tmdbId, string name, string? tmdbProfilePath)
        {
            await PersonCacheLock.WaitAsync();
            try
            {
                if (PersonCache.TryGetValue(name, out long cachedId))
                {
                    return cachedId;
                }
            }
            finally 
            { 
                PersonCacheLock.Release(); 
            }

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

            if (newId.HasValue)
            {
                await PersonCacheLock.WaitAsync();
                try 
                { 
                    PersonCache[name] = newId.Value;
                }
                finally
                { 
                    PersonCacheLock.Release();
                }
            }

            return newId;
        }

        private static int ExtractYear(string? date)
        {
            if (string.IsNullOrEmpty(date)) return 0;
            return DateTime.TryParse(date, out var dt) ? dt.Year : 0;
        }
    }

    //-----------Special models----------

    class GenreRow 
    { 
        public long id { get; set; }
        public string name { get; set; } 
    }
    class PersonRow 
    {
        public long id { get; set; }
        public string name { get; set; } 
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
        public string status { get; set; }
        public int runtime { get; set; }
        public double vote_average { get; set; }
        public string release_date { get; set; }
        public List<Genre> genres { get; set; }
    }

    public class Genre 
    {
        public string name { get; set; }
    }

    public class TmdbCredits
    {
        public List<TmdbCastMember> cast { get; set; }
        public List<TmdbCrewMember> crew { get; set; }
    }

    public class TmdbCastMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public string character { get; set; }
        public string profile_path { get; set; }
    }

    public class TmdbCrewMember
    {
        public int id { get; set; }
        public string name { get; set; }
        public string job { get; set; }
        public string profile_path { get; set; }
    }

    public class TmdbPerson
    {
        public int id { get; set; }
        public string name { get; set; }
        public string profile_path { get; set; }
    }

    public class TmdbPersonDetails
    {
        public string biography { get; set; }
        public string birthday { get; set; }
        public string place_of_birth { get; set; }
        public int gender { get; set; }
    }
}