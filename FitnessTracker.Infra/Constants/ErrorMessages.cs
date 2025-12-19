namespace FitnessTracker.Infra.Constants;

public static class ErrorMessages
{
    // Users

    public const string DuplicatedName = "Duplicated name";
    public const string DuplicatedEmail = "Duplicated email";
    public const string NameTaken = "Name already taken";
    public const string EmailTaken = "Email already taken";

    public const string UserIdNotFound = "User with ID {0} not found";
    public const string UserEmailNotFound = "User with email {0} not found";
    public const string InvalidCredentials = "Invalid credentials";
    public const string UnauthorizedAccountDeletion = "Unauthorized to delete another user's account";

    // Workouts

    public const string WorkoutIdNotFound = "Workout with id {0} not found";
    public const string WorkoutIdsNotFound = "Invalid workouts IDs found";
    public const string DuplicatedWorkoutStartTime = "Workout already exists at this time.";

    // Exercises

    public const string ExerciseIdNotFound = "Exercise with id {0} not found";
    public const string ExerciseIdsNotFound = "Invalid exercises IDs found";
    public const string ExercisesInUse = "Exercises  used by existing workouts";
}
