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
    }

    public static class Workouts
    {
        public const string Base = "api/workouts";
    }

    public static class Exercises
    {
        public const string Base = "api/exercises";
    }

    public static class Goals
    {
        public const string Base = "api/goals";
    }

    public static class FoodItems
    {
        public const string Base = "api/foodItems";
    }
}
