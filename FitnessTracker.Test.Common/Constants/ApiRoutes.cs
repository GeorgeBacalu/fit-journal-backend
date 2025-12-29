namespace FitnessTracker.Test.Common.Constants;

public static class ApiRoutes
{
    public static class Auth
    {
        public const string Base = "api/Auth";
        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }

    public static class Users
    {
        public const string Base = "api/User";
    }

    public static class Workouts
    {
        public const string Base = "api/Workout";
    }

    public static class Exercises
    {
        public const string Base = "api/Exercise";
    }

    public static class Goals
    {
        public const string Base = "api/Goal";
    }

    public static class FoodItems
    {
        public const string Base = "api/FoodItem";
    }

    public static class FoodLogs
    {
        public const string Base = "api/FoodLog";
    }

    public static class ProgressLogs
    {
        public const string Base = "api/ProgressLog";
    }

    public static class WorkoutExercises
    {
        public const string Base = "api/WorkoutExercise";
    }
}
