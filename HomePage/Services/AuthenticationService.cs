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

        public Task<string?> Register(string email, string password, string displayName, string dateOfBirth)
        => Supabase.Register(email, password, displayName, dateOfBirth);

        public Task<string?> Login(string email, string password)
            => Supabase.Login(email, password);
    }
}