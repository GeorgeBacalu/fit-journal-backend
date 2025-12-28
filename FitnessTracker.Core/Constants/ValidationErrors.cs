namespace FitnessTracker.Core.Constants;

public static class ValidationErrors
{
    public static class Common
    {
        public const string NameRequired = "Name is required";
        public const string NameTooLong = "Name exceeds {MaxLength} characters";
        public const string NameTaken = "Name already taken";
    }

    public static class Users
    {
        public const string EmailRequired = "Email is required";
        public const string InvalidEmail = "Invalid email format";
        public const string EmailTooLong = "Email exceeds {MaxLength} characters";
        public const string EmailTaken = "Email already taken";

        public const string PasswordRequired = "Password is required";
        public const string InvalidPassword = "Invalid password";
        public const string InvalidPasswordLength = "Password length must be {MinLength}–{MaxLength}";

        public const string ConfirmPassword = "Confirm your password";
        public const string PasswordsMismatch = "Passwords don't match";

        public const string PhoneRequired = "Phone is required";
        public const string InvalidPhone = "Invalid phone";
        public const string PhoneTooLong = "Phone exceeds {MaxLength} characters";

        public const string BirthdayRequired = "Birthday is required";
        public const string InvalidBirthday = "Invalid birthday format";
        public const string FutureBirthday = "Birthday can't be in the future";
        public const string AgeRestriction = "You must be at least 13 years old";

        public const string HeightRequired = "Height is required";
        public const string HeightOutOfRange = "Height must be {Min}-{Max} cm";

        public const string WeightRequired = "Weight is required";
        public const string WeightOutOfRange = "Weight must be {Min}-{Max} kg";

        public const string GenderRequired = "Gender is required";
        public const string InvalidGender = "Invalid gender";
    }

    public static class Workouts
    {
        public const string InvalidDescriptionLength = "Description length must be {MinLength}–{MaxLength}";
        public const string InvalidNotesLength = "Notes length must be {MinLength}–{MaxLength}";

        public const string DurationRequired = "Duration is required";
        public const string DurationOutOfRange = "Duration must be {Min}-{Max} minutes";

        public const string StartDateRequired = "Start date is required";

        public const string BeforeRegistration = "Workout start date can't precede registration date";
        public const string DuplicatedStartedAt = "User already has a workout at this time";

        public const string IdRequired = "Workout id is required";
        public const string InvalidId = "Invalid workout id";

        public const string IdsRequired = "No workout ids provided";
        public const string DuplicatedIds = "Duplicated workout ids provided";
    }

    public static class Exercises
    {
        public const string InvalidDescriptionLength = "Description length must be {MinLength}–{MaxLength}";
        public const string InvalidNotesLength = "Notes length must be {MinLength}–{MaxLength}";

        public const string MuscleGroupRequired = "Muscle group is required";
        public const string InvalidMuscleGroup = "Invalid muscle group";

        public const string DifficultyLevelRequired = "Difficulty level is required";
        public const string InvalidDifficultyLevel = "Invalid difficulty level";

        public const string AlreadyInUse = "Exercise is used by workouts";

        public const string IdRequired = "Exercise id is required";
        public const string InvalidId = "Invalid exercise id";

        public const string IdsRequired = "No exercise ids provided";
        public const string DuplicatedIds = "Duplicated exercise ids provided";
    }

    public static class WorkoutExercises
    {
        public const string SetsRequired = "Sets are required";
        public const string SetsOutOfRange = "Sets must be {Min}-{Max}";

        public const string RepsRequired = "Reps are required";
        public const string RepsOutOfRange = "Reps must be {Min}-{Max}";

        public const string WeightUsedRequired = "Weight used is required";
        public const string WeightUsedOutOfRange = "Weight used must be {Min}-{Max} kg";

        public const string AlreadyAdded = "Exercise is already added to this workout";
        public const string ExerciseNotInWorkout = "Exercise is not part of this workout";

        public const string IdRequired = "Workout exercise id is required";
        public const string InvalidId = "Invalid workout exercise id";
    }

    public static class Goals
    {
        public const string InvalidDescriptionLength = "Description length must be {MinLength}–{MaxLength}";
        public const string InvalidNotesLength = "Notes length must be {MinLength}–{MaxLength}";

        public const string TypeRequired = "Goal type is required";
        public const string InvalidType = "Invalid goal type";

        public const string TargetWeightRequired = "Target weight is required";
        public const string InvalidTargetWeight = "Target weight must be {Min}-{Max} kg";

        public const string StartDateRequired = "Start date is required";
        public const string StartBeforeEnd = "Start date must be before end date";

        public const string EndDateRequired = "End date is required";
        public const string InvalidEndDate = "End date can't be in the past";

        public const string BeforeRegistration = "Goal start date can't precede registration date";
        public const string WeightTargetTypeMismatch = "Weight target conflicts with goal type";
        public const string SameTypeGoalOverlap = "Same type user goals can't overlap";

        public const string IdRequired = "Goal id is required";
        public const string InvalidId = "Invalid goal id";

        public const string IdsRequired = "No goal ids provided";
        public const string DuplicatedIds = "Duplicated goal ids provided";
    }

    public static class FoodItems
    {
        public const string CaloriesRequired = "Calories are required";
        public const string InvalidCalories = "Calories count must be positive";

        public const string ProteinRequired = "Protein is required";
        public const string InvalidProtein = "Protein count must be positive";

        public const string CarbsRequired = "Carbs are required";
        public const string InvalidCarbs = "Carbs count must be positive";

        public const string FatRequired = "Fat is required";
        public const string InvalidFat = "Fat count must be positive";

        public const string CategoryRequired = "Category is required";
        public const string InvalidCategory = "Invalid category";

        public const string BrandRequired = "Brand is required";
        public const string InvalidBrand = "Invalid brand";

        public const string IdRequired = "Food item id is required";
        public const string InvalidId = "Invalid food item id";

        public const string IdsRequired = "No food item ids provided";
        public const string DuplicatedIds = "Duplicated food item ids provided";
    }

    public static class FoodLogs
    {
        public const string DateRequired = "Food log date is required";
        public const string FutureDate = "Food log can't be in the future";

        public const string ServingsRequired = "Servings are required";
        public const string InvalidServings = "Servings must be positive";

        public const string QuantityRequired = "Quantity is required";
        public const string QuantityOutOfRange = "Quantity must be {Min}-{Max} g";

        public const string BeforeRegistration = "Food log date can't precede registration date";

        public const string IdRequired = "Food log id is required";
        public const string InvalidId = "Invalid food log id";

        public const string IdsRequired = "No food log ids provided";
        public const string DuplicatedIds = "Duplicated food log ids provided";
    }

    public static class ProgressLogs
    {
        public const string DateRequired = "Progress log date is required";
        public const string FutureDate = "Progress log date can't be in the future";

        public const string WeightRequired = "Weight is required";
        public const string WeightOutOfRange = "Weight must be {Min}-{Max} kg";

        public const string BodyFatRequired = "Body fat percentage is required";
        public const string BodyFatOutOfRange = "Body fat percentage must be {Min}-{Max} %";

        public const string WaistCmRequired = "Waist circumference is required";
        public const string WaistCmOutOfRange = "Waist circumference must be {Min}-{Max} cm";

        public const string ChestCmRequired = "Chest circumference is required";
        public const string ChestCmOutOfRange = "Chest circumference must be {Min}-{Max} cm";

        public const string ArmsCmRequired = "Arms circumference is required";
        public const string ArmsCmOutOfRange = "Arms circumference must be {Min}-{Max} cm";

        public const string BeforeRegistration = "Progress log date can't precede registration date";
        public const string DuplicatedDailyLog = "Only one progress log per day is allowed";
        public const string WeightChangeTooHigh = "Weight change exceeds allowed limit";

        public const string IdRequired = "Progress log id is required";
        public const string InvalidId = "Invalid progress log id";

        public const string IdsRequired = "No progress log ids provided";
        public const string DuplicatedIds = "Duplicated progress log ids provided";
    }
}
