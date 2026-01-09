using FitJournal.Core.Results;

namespace FitJournal.Core.Constants;

public static class ValidationErrors
{
    public static class Common
    {
        static Error E(string name, string message) => Error.Create(typeof(Common), name, message);

        public static readonly Error NameRequired = E(nameof(NameRequired), "Name is required");
        public static readonly Error NameTooLong = E(nameof(NameTooLong), "Name exceeds {MaxLength} characters");
        public static readonly Error NameTaken = E(nameof(NameTaken), "Name already taken");
    }

    public static class Users
    {
        static Error E(string name, string message) => Error.Create(typeof(Users), name, message);

        public static readonly Error EmailRequired = E(nameof(EmailRequired), "Email is required");
        public static readonly Error InvalidEmail = E(nameof(InvalidEmail), "Invalid email format");
        public static readonly Error EmailTooLong = E(nameof(EmailTooLong), "Email exceeds {MaxLength} characters");
        public static readonly Error EmailTaken = E(nameof(EmailTaken), "Email already taken");

        public static readonly Error PasswordRequired = E(nameof(PasswordRequired), "Password is required");
        public static readonly Error InvalidPassword = E(nameof(InvalidPassword), "Invalid password");
        public static readonly Error InvalidPasswordLength = E(nameof(InvalidPasswordLength), "Password length must be {MinLength}–{MaxLength}");

        public static readonly Error ConfirmPassword = E(nameof(ConfirmPassword), "Confirm your password");
        public static readonly Error PasswordsMismatch = E(nameof(PasswordsMismatch), "Passwords don't match");

        public static readonly Error PhoneRequired = E(nameof(PhoneRequired), "Phone is required");
        public static readonly Error InvalidPhone = E(nameof(InvalidPhone), "Invalid phone");
        public static readonly Error PhoneTooLong = E(nameof(PhoneTooLong), "Phone exceeds {MaxLength} characters");

        public static readonly Error BirthdayRequired = E(nameof(BirthdayRequired), "Birthday is required");
        public static readonly Error InvalidBirthday = E(nameof(InvalidBirthday), "Invalid birthday format");
        public static readonly Error FutureBirthday = E(nameof(FutureBirthday), "Birthday can't be in the future");
        public static readonly Error AgeRestriction = E(nameof(AgeRestriction), "You must be at least 13 years old");

        public static readonly Error HeightRequired = E(nameof(HeightRequired), "Height is required");
        public static readonly Error HeightOutOfRange = E(nameof(HeightOutOfRange), "Height must be {Min}-{Max} cm");

        public static readonly Error WeightRequired = E(nameof(WeightRequired), "Weight is required");
        public static readonly Error WeightOutOfRange = E(nameof(WeightOutOfRange), "Weight must be {Min}-{Max} kg");

        public static readonly Error GenderRequired = E(nameof(GenderRequired), "Gender is required");
        public static readonly Error InvalidGender = E(nameof(InvalidGender), "Invalid gender");
    }

    public static class Workouts
    {
        static Error E(string name, string message) => Error.Create(typeof(Workouts), name, message);

        public static readonly Error InvalidDescriptionLength = E(nameof(InvalidDescriptionLength), "Description length must be {MinLength}–{MaxLength}");
        public static readonly Error InvalidNotesLength = E(nameof(InvalidNotesLength), "Notes length must be {MinLength}–{MaxLength}");

        public static readonly Error DurationRequired = E(nameof(DurationRequired), "Duration is required");
        public static readonly Error DurationOutOfRange = E(nameof(DurationOutOfRange), "Duration must be {Min}-{Max} minutes");

        public static readonly Error StartDateRequired = E(nameof(StartDateRequired), "Start date is required");

        public static readonly Error BeforeRegistration = E(nameof(BeforeRegistration), "Workout start date can't precede registration date");
        public static readonly Error DuplicatedStartedAt = E(nameof(DuplicatedStartedAt), "User already has a workout at this time");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Workout id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid workout id");

        public static readonly Error IdsRequired = E(nameof(IdsRequired), "No workout ids provided");
        public static readonly Error DuplicatedIds = E(nameof(DuplicatedIds), "Duplicated workout ids provided");
    }

    public static class Exercises
    {
        static Error E(string name, string message) => Error.Create(typeof(Exercises), name, message);

        public static readonly Error InvalidDescriptionLength = E(nameof(InvalidDescriptionLength), "Description length must be {MinLength}–{MaxLength}");
        public static readonly Error InvalidNotesLength = E(nameof(InvalidNotesLength), "Notes length must be {MinLength}–{MaxLength}");

        public static readonly Error MuscleGroupRequired = E(nameof(MuscleGroupRequired), "Muscle group is required");
        public static readonly Error InvalidMuscleGroup = E(nameof(InvalidMuscleGroup), "Invalid muscle group");

        public static readonly Error DifficultyLevelRequired = E(nameof(DifficultyLevelRequired), "Difficulty level is required");
        public static readonly Error InvalidDifficultyLevel = E(nameof(InvalidDifficultyLevel), "Invalid difficulty level");

        public static readonly Error AlreadyInUse = E(nameof(AlreadyInUse), "Exercise is used by workouts");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Exercise id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid exercise id");

        public static readonly Error IdsRequired = E(nameof(IdsRequired), "No exercise ids provided");
        public static readonly Error DuplicatedIds = E(nameof(DuplicatedIds), "Duplicated exercise ids provided");
    }

    public static class WorkoutExercises
    {
        static Error E(string name, string message) => Error.Create(typeof(WorkoutExercises), name, message);

        public static readonly Error SetsRequired = E(nameof(SetsRequired), "Sets are required");
        public static readonly Error SetsOutOfRange = E(nameof(SetsOutOfRange), "Sets must be {Min}-{Max}");

        public static readonly Error RepsRequired = E(nameof(RepsRequired), "Reps are required");
        public static readonly Error RepsOutOfRange = E(nameof(RepsOutOfRange), "Reps must be {Min}-{Max}");

        public static readonly Error WeightUsedRequired = E(nameof(WeightUsedRequired), "Weight used is required");
        public static readonly Error WeightUsedOutOfRange = E(nameof(WeightUsedOutOfRange), "Weight used must be {Min}-{Max} kg");

        public static readonly Error AlreadyAdded = E(nameof(AlreadyAdded), "Exercise already added to this workout");
        public static readonly Error ExerciseNotInWorkout = E(nameof(ExerciseNotInWorkout), "Exercise not added to this workout");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Workout exercise id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid workout exercise id");
    }

    public static class Goals
    {
        static Error E(string name, string message) => Error.Create(typeof(Goals), name, message);

        public static readonly Error InvalidDescriptionLength = E(nameof(InvalidDescriptionLength), "Description length must be {MinLength}–{MaxLength}");
        public static readonly Error InvalidNotesLength = E(nameof(InvalidNotesLength), "Notes length must be {MinLength}–{MaxLength}");

        public static readonly Error TypeRequired = E(nameof(TypeRequired), "Goal type is required");
        public static readonly Error InvalidType = E(nameof(InvalidType), "Invalid goal type");

        public static readonly Error TargetWeightRequired = E(nameof(TargetWeightRequired), "Target weight is required");
        public static readonly Error InvalidTargetWeight = E(nameof(InvalidTargetWeight), "Target weight must be {Min}-{Max} kg");

        public static readonly Error StartDateRequired = E(nameof(StartDateRequired), "Start date is required");
        public static readonly Error StartBeforeEnd = E(nameof(StartBeforeEnd), "Start date must be before end date");

        public static readonly Error EndDateRequired = E(nameof(EndDateRequired), "End date is required");
        public static readonly Error InvalidEndDate = E(nameof(InvalidEndDate), "End date can't be in the past");

        public static readonly Error BeforeRegistration = E(nameof(BeforeRegistration), "Goal start date can't precede registration date");
        public static readonly Error WeightTargetTypeMismatch = E(nameof(WeightTargetTypeMismatch), "Weight target conflicts with goal type");
        public static readonly Error SameTypeGoalOverlap = E(nameof(SameTypeGoalOverlap), "Same type user goals can't overlap");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Goal id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid goal id");

        public static readonly Error IdsRequired = E(nameof(IdsRequired), "No goal ids provided");
        public static readonly Error DuplicatedIds = E(nameof(DuplicatedIds), "Duplicated goal ids provided");
    }

    public static class FoodItems
    {
        static Error E(string name, string message) => Error.Create(typeof(FoodItems), name, message);

        public static readonly Error CaloriesRequired = E(nameof(CaloriesRequired), "Calories are required");
        public static readonly Error InvalidCalories = E(nameof(InvalidCalories), "Calories count must be positive");

        public static readonly Error ProteinRequired = E(nameof(ProteinRequired), "Protein is required");
        public static readonly Error InvalidProtein = E(nameof(InvalidProtein), "Protein count must be positive");

        public static readonly Error CarbsRequired = E(nameof(CarbsRequired), "Carbs are required");
        public static readonly Error InvalidCarbs = E(nameof(InvalidCarbs), "Carbs count must be positive");

        public static readonly Error FatRequired = E(nameof(FatRequired), "Fat is required");
        public static readonly Error InvalidFat = E(nameof(InvalidFat), "Fat count must be positive");

        public static readonly Error CategoryRequired = E(nameof(CategoryRequired), "Category is required");
        public static readonly Error InvalidCategory = E(nameof(InvalidCategory), "Invalid category");

        public static readonly Error BrandRequired = E(nameof(BrandRequired), "Brand is required");
        public static readonly Error InvalidBrand = E(nameof(InvalidBrand), "Invalid brand");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Food item id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid food item id");

        public static readonly Error IdsRequired = E(nameof(IdsRequired), "No food item ids provided");
        public static readonly Error DuplicatedIds = E(nameof(DuplicatedIds), "Duplicated food item ids provided");
    }

    public static class FoodLogs
    {
        static Error E(string name, string message) => Error.Create(typeof(FoodLogs), name, message);

        public static readonly Error DateRequired = E(nameof(DateRequired), "Food log date is required");
        public static readonly Error FutureDate = E(nameof(FutureDate), "Food log can't be in the future");

        public static readonly Error ServingsRequired = E(nameof(ServingsRequired), "Servings are required");
        public static readonly Error InvalidServings = E(nameof(InvalidServings), "Servings must be positive");

        public static readonly Error QuantityRequired = E(nameof(QuantityRequired), "Quantity is required");
        public static readonly Error QuantityOutOfRange = E(nameof(QuantityOutOfRange), "Quantity must be {Min}-{Max} g");

        public static readonly Error BeforeRegistration = E(nameof(BeforeRegistration), "Food log date can't precede registration date");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Food log id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid food log id");

        public static readonly Error IdsRequired = E(nameof(IdsRequired), "No food log ids provided");
        public static readonly Error DuplicatedIds = E(nameof(DuplicatedIds), "Duplicated food log ids provided");
    }

    public static class ProgressLogs
    {
        static Error E(string name, string message) => Error.Create(typeof(ProgressLogs), name, message);

        public static readonly Error DateRequired = E(nameof(DateRequired), "Progress log date is required");
        public static readonly Error FutureDate = E(nameof(FutureDate), "Progress log date can't be in the future");

        public static readonly Error WeightRequired = E(nameof(WeightRequired), "Weight is required");
        public static readonly Error WeightOutOfRange = E(nameof(WeightOutOfRange), "Weight must be {Min}-{Max} kg");

        public static readonly Error BodyFatRequired = E(nameof(BodyFatRequired), "Body fat percentage is required");
        public static readonly Error BodyFatOutOfRange = E(nameof(BodyFatOutOfRange), "Body fat percentage must be {Min}-{Max} %");

        public static readonly Error WaistCmRequired = E(nameof(WaistCmRequired), "Waist circumference is required");
        public static readonly Error WaistCmOutOfRange = E(nameof(WaistCmOutOfRange), "Waist circumference must be {Min}-{Max} cm");

        public static readonly Error ChestCmRequired = E(nameof(ChestCmRequired), "Chest circumference is required");
        public static readonly Error ChestCmOutOfRange = E(nameof(ChestCmOutOfRange), "Chest circumference must be {Min}-{Max} cm");

        public static readonly Error ArmsCmRequired = E(nameof(ArmsCmRequired), "Arms circumference is required");
        public static readonly Error ArmsCmOutOfRange = E(nameof(ArmsCmOutOfRange), "Arms circumference must be {Min}-{Max} cm");

        public static readonly Error BeforeRegistration = E(nameof(BeforeRegistration), "Progress log date can't precede registration date");
        public static readonly Error DuplicatedDailyLog = E(nameof(DuplicatedDailyLog), "Only one progress log per day is allowed");
        public static readonly Error WeightChangeTooHigh = E(nameof(WeightChangeTooHigh), "Weight change exceeds allowed limit");

        public static readonly Error IdRequired = E(nameof(IdRequired), "Progress log id is required");
        public static readonly Error InvalidId = E(nameof(InvalidId), "Invalid progress log id");

        public static readonly Error IdsRequired = E(nameof(IdsRequired), "No progress log ids provided");
        public static readonly Error DuplicatedIds = E(nameof(DuplicatedIds), "Duplicated progress log ids provided");
    }
}
