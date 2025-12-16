namespace FitnessTracker.Infra.Constants;

public static class ErrorMessages
{
    public const string DuplicatedName = "Duplicated name";
    public const string DuplicatedEmail = "Duplicated email";
    public const string NameTaken = "Name already taken";
    public const string EmailTaken = "Email already taken";

    public const string AgeRestriction = "You must be at least 13 years old";

    public const string UserIdNotFound = "User with ID {0} not found";
    public const string UserEmailNotFound = "User with email {0} not found";
    public const string WorkoutIdNotFound = "Workout with id {0} not found";
    public const string ExerciseIdNotFound = "Exercise with id {0} not found";

    public const string InvalidCredentials = "Invalid credentials";

    public const string WorkoutBeforeRegistration = "Workout date can't be before user registration date";
    public const string DuplicateWorkoutStartTime = "Users can't log multiple workouts with the same start time";
}
