namespace FitnessTracker.Core.Constants;

public static class BusinessErrors
{
    public static class Users
    {
        public const string InvalidCredentials = "Invalid credentials";

        public const string IdNotFound = "User with id {0} not found";
        public const string EmailNotFound = "User with email {0} not found";
    }

    public static class Workouts
    {
        public const string IdNotFound = "Workout with id {0} not found";
        public const string IdsNotFound = "Invalid workouts ids found";
    }

    public static class Exercises
    {
        public const string IdNotFound = "Exercise with id {0} not found";
        public const string IdsNotFound = "Invalid exercises ids found";
    }

    public static class WorkoutExercises
    {
        public const string UnauthorizedAccess = "You can't access another user's workout";

        public const string IdNotFound = "Workout exercise with id {0} not found";
        public const string IdsNotFound = "Invalid workout exercises ids found";
    }

    public static class Goals
    {
        public const string IdNotFound = "Goal with id {0} not found";
        public const string IdsNotFound = "Invalid goals ids found";
    }

    public static class FoodItems
    {
        public const string IdNotFound = "Food item with id {0} not found";
        public const string IdsNotFound = "Invalid food items ids found";
    }

    public static class FoodLogs
    {
        public const string IdNotFound = "Food log with id {0} not found";
        public const string IdsNotFound = "Invalid food logs ids found";
    }

    public static class ProgressLogs
    {
        public const string IdNotFound = "Progress log with id {0} not found";
        public const string IdsNotFound = "Invalid progress logs ids found";
    }
}
