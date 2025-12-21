namespace FitnessTracker.Infra.Constants;

public static class ErrorMessages
{
    public static class Users
    {
        public const string NameTaken = "Name already taken";
        public const string EmailTaken = "Email already taken";

        public const string IdNotFound = "User with ID {0} not found";
        public const string EmailNotFound = "User with email {0} not found";

        public const string InvalidCredentials = "Invalid credentials";
    }

    public static class Workouts
    {
        public const string IdNotFound = "Workout with id {0} not found";
        public const string IdsNotFound = "Invalid workouts IDs found";

        public const string BeforeRegistration = "Workout start date can't precede registration date";

        public const string UnauthorizedEdit = "Unauthorized to edit another user's workout";
        public const string UnauthorizedRemove = "Unauthorized to edit another user's workout";
    }

    public static class Exercises
    {
        public const string IdNotFound = "Exercise with id {0} not found";
        public const string IdsNotFound = "Invalid exercises IDs found";

        public const string AlreadyInUse = "Exercise is used by workouts";
    }

    public static class Goals
    {
        public const string IdNotFound = "Goal with id {0} not found";
        public const string IdsNotFound = "Invalid goal IDs found";

        public const string BeforeRegistration = "Goal start date can't precede registration date";

        public const string UnauthorizedEdit = "Unauthorized to edit another user's goal";
        public const string UnauthorizedRemove = "Unauthorized to remove another user's goal";
    }

    public static class FoodItems
    {
        public const string IdNotFound = "Food item with id {0} not found";
        public const string IdsNotFound = "Invalid food item IDs found";
    }

    public static class FoodLogs
    {
        public const string IdNotFound = "Food log with id {0} not found";
        public const string IdsNotFound = "Invalid food log IDs found";

        public const string BeforeRegistration = "Food log date can't precede registration date";

        public const string UnauthorizedEdit = "Unauthorized to edit another user's food log";
        public const string UnauthorizedRemove = "Unauthorized to remove another user's food log";
    }

    public static class MeasurementLogs
    {
        public const string IdNotFound = "Measurement log with id {0} not found";

        public const string BeforeRegistration = "Measurement log date can't precede registration date";

        public const string UnauthorizedEdit = "Unauthorized to edit another user's measurement log";
    }
}
