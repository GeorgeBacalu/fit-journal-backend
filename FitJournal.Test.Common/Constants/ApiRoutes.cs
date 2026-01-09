namespace FitJournal.Test.Common.Constants;

public static class ApiRoutes
{
    public static class Auth
    {
        public const string Base = "api/v1/Auth";
        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }

    public static class Users
    {
        public const string Base = "api/v1/User";
    }

    public static class Workouts
    {
        public const string Base = "api/v1/Workout";
    }

    public static class Exercises
    {
        public const string Base = "api/v1/Exercise";
    }

    public static class Goals
    {
        public const string Base = "api/v1/Goal";
    }

    public static class FoodItems
    {
        public const string Base = "api/v1/FoodItem";
    }

    public static class FoodLogs
    {
        public const string Base = "api/v1/FoodLog";
    }

    public static class ProgressLogs
    {
        public const string Base = "api/v1/ProgressLog";
    }

    public static class WorkoutExercises
    {
        public const string Base = "api/v1/WorkoutExercise";
    }

    public static class RequestLogs
    {
        public const string Base = "api/v1/RequestLog";
    }
}
