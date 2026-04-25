using System.Threading.Tasks;

namespace App.Services
{
    public class AuthenticationService
    {
        private readonly SupabaseClient Supabase;

        public AuthenticationService(SupabaseClient supabase)
        {
            Supabase = supabase;
        }

        public Task<bool> Register(string email, string password, string displayName, string dateOfBirth)
            => Supabase.Register(email, password, displayName, dateOfBirth);

        public Task<string?> Login(string email, string password)
            => Supabase.Login(email, password);

        // GetAll<T> is available via _supabase.GetAll<T>() anywhere you need it.
        // Expose it here only if callers need it through AuthenticationService specifically.
    }
}