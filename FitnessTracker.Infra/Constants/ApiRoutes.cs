namespace FitnessTracker.Infra.Constants;

public static class ApiRoutes
{
    public static class Auth
    {
        public const string Base = "api/auth";
        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }

    public static class Users
    {
        public const string Base = "api/users";
        public const string Profile = $"{Base}/profile";
    }
}
