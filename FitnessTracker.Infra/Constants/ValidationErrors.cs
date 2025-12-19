namespace FitnessTracker.Infra.Constants;

public static class ValidationErrors
{
    // Users

    public const string NameRequired = "Name is required";
    public const string InvalidNameLength = "Name must have max {MaxLength} characters";
    public const string NameTaken = "Name already taken";

    public const string EmailRequired = "Email is required";
    public const string InvalidEmail = "Invalid email address";
    public const string InvalidEmailLength = "Email must have max {MaxLength} characters";
    public const string EmailTaken = "Email already taken";

    public const string PasswordRequired = "Password is required";
    public const string InvalidPassword = "Invalid password";
    public const string InvalidPasswordLength = "Password must have between {MinLength} and {MaxLength} characters";

    public const string ConfirmPassword = "Confirm your password";
    public const string PasswordsMismatch = "Passwords don't match";

    public const string PhoneRequired = "Phone is required";
    public const string InvalidPhone = "Invalid phone";
    public const string InvalidPhoneLength = "Phone must have max {MaxLength} characters";

    public const string BirthdayRequired = "Birthday is required";
    public const string InvalidBirthday = "Birthday can't be in the future";
    public const string AgeRestriction = "You must be at least 13 years old";

    public const string HeightRequired = "Height is required";
    public const string InvalidHeight = "Height must be between {Min} and {Max} cm";
    
    public const string WeightRequired = "Weight is required";
    public const string InvalidWeight = "Weight must be between {Min} and {Max} kg";
    
    public const string GenderRequired = "Gender is required";

    // Workouts

    public const string InvalidDescriptionLength = "Description must have between {MinLength} and {MaxLength} characters";

    public const string InvalidNotesLength = "Notes must have between {MinLength} and {MaxLength} characters";

    public const string DurationRequired = "Duration is required";
    public const string InvalidDuration = "Duration must be between {Min} and {Max} minutes";

    public const string StartDateRequired = "Start date is required";
    public const string InvalidStartDate = "Start date can't be in the future";

    public const string WorkoutIdRequired = "Workout ID is required";
    public const string InvalidWorkoutId = "Invalid workout ID";

    public const string WorkoutIdsRequired = "No workout IDs provided";
    public const string DuplicatedWorkoutIds = "Request contains duplicated workout IDs";

    // Exercises

    public const string MuscleGroupRequired = "Muscle group is required";
    
    public const string DifficultyLevelRequired = "Difficulty level is required";

    public const string ExerciseIdRequired = "Exercise ID is required";
    public const string InvalidExerciseId = "Invalid exercise ID";

    public const string ExerciseIdsRequired = "No exercise IDs provided";

    // Goals

    public const string GoalTypeRequired = "Goal type is required";

    public const string TargetWeightRequired = "Target weight is required";
    public const string InvalidTargetWeight = "Trget weight must be between {Min} and {Max} kg";

    public const string StartBeforeEnd = "Start date must be earlier than the end date";

    public const string EndDateRequired = "End date is required";
    public const string InvalidEndDate = "End date can't be in the past";

    public const string GoalIdRequired = "Goal ID is required";
    public const string InvalidGoalId = "Invalid goal ID";

    // Food Items

    public const string CaloriesRequired = "Calories are required";
    public const string InvalidCalories = "Calories must be a positive value";

    public const string ProteinRequired = "Protein is required";
    public const string InvalidProtein = "Protein must be a positive value";

    public const string CarbsRequired = "Carbohydrates are required";
    public const string InvalidCarbs = "Carbohydrates must be a positive value";

    public const string FatRequired = "Fat is required";
    public const string InvalidFat = "Fat must be a positive value";
}
