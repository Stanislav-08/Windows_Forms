namespace App.Services
{
    public static class Session
    {
        //Auth token
        public static string AccessToken { get; set; }

        //Username
        public static string DisplayName { get; set; }

        //Login check
        public static bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);
    }
}