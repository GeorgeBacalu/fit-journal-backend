namespace FitnessTracker.Infra.Constants;

public static class ValidationErrors
{
    public static class Common
    {
        public const string NameRequired = "Name is required";
        public const string InvalidNameLength = "Name must have max {MaxLength} characters";
        public const string NameTaken = "Name already taken";
    }

    public static class Users
    {
        public const string EmailRequired = "Email is required";
        public const string InvalidEmail = "Invalid email format";
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
        public const string InvalidBirthday = "Invalid birthday format";
        public const string FutureBirthday = "Birthday can't be in the future";
        public const string AgeRestriction = "You must be at least 13 years old";

        public const string HeightRequired = "Height is required";
        public const string InvalidHeight = "Height must be between {Min} and {Max} cm";

        public const string WeightRequired = "Weight is required";
        public const string InvalidWeight = "Weight must be between {Min} and {Max} kg";

        public const string GenderRequired = "Gender is required";
    }

    public static class Workouts
    {
        public const string InvalidDescriptionLength = "Description must have between {MinLength} and {MaxLength} characters";

        public const string InvalidNotesLength = "Notes must have between {MinLength} and {MaxLength} characters";

        public const string DurationRequired = "Duration is required";
        public const string InvalidDuration = "Duration must be between {Min} and {Max} minutes";

        public const string StartDateRequired = "Start date is required";
        public const string FutureStartDate = "Start date can't be in the future";

        public const string DuplicatedStartTime = "User already has a workout at this time";

        public const string IdRequired = "Workout ID is required";
        public const string InvalidId = "Invalid workout ID";

        public const string IdsRequired = "No workout IDs provided";
        public const string DuplicatedIds = "Duplicated workout IDs provided";
    }

    public static class Exercises
    {
        public const string InvalidDescriptionLength = "Description must have between {MinLength} and {MaxLength} characters";

        public const string InvalidNotesLength = "Notes must have between {MinLength} and {MaxLength} characters";

        public const string MuscleGroupRequired = "Muscle group is required";

        public const string DifficultyLevelRequired = "Difficulty level is required";

        public const string IdRequired = "Exercise ID is required";
        public const string InvalidId = "Invalid exercise ID";

        public const string IdsRequired = "No exercise IDs provided";
        public const string DuplicatedIds = "Duplicated exercise IDs provided";
    }

    public static class Goals
    {
        public const string InvalidDescriptionLength = "Description must have between {MinLength} and {MaxLength} characters";

        public const string TypeRequired = "Goal type is required";

        public const string TargetWeightRequired = "Target weight is required";
        public const string InvalidTargetWeight = "Trget weight must be between {Min} and {Max} kg";

        public const string StartDateRequired = "Start date is required";
        public const string StartBeforeEnd = "Start date must be earlier than the end date";

        public const string EndDateRequired = "End date is required";
        public const string InvalidEndDate = "End date can't be in the past";

        public const string IdRequired = "Goal ID is required";
        public const string InvalidId = "Invalid goal ID";

        public const string IdsRequired = "No goal IDs provided";
        public const string DuplicatedIds = "Duplicated goal IDs provided";
    }

    public static class FoodItems
    {
        public const string CaloriesRequired = "Calories are required";
        public const string InvalidCalories = "Calories must be a positive value";

        public const string ProteinRequired = "Protein is required";
        public const string InvalidProtein = "Protein must be a positive value";

        public const string CarbsRequired = "Carbs are required";
        public const string InvalidCarbs = "Carbs must be a positive value";

        public const string FatRequired = "Fat is required";
        public const string InvalidFat = "Fat must be a positive value";

        public const string IdRequired = "Food item ID is required";
        public const string InvalidId = "Invalid food item ID";

        public const string IdsRequired = "Invalid food item ID";
        public const string DuplicatedIds = "Duplicated food item IDs provided";
    }

    public static class FoodLogs
    {
        public const string DateRequired = "Date is required";
        public const string FutureDate = "Date can't be in the future";

        public const string ServingsRequired = "Servings are required";
        public const string InvalidServings = "Servings must be a positive value";

        public const string QuantityRequired = "Quantity is required";
        public const string InvalidQuantity = "Quantity must be between {Min} and {Max} g";

        public const string IdRequired = "Food log ID is required";
        public const string InvalidId = "Invalid food log ID";

        public const string IdsRequired = "Invalid food log ID";
        public const string DuplicatedIds = "Duplicated food log IDs provided";
    }
}
