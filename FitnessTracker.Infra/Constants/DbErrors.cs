namespace FitnessTracker.Infra.Constants;

public static class DbErrors
{
    public static class Users
    {
        public const string NameTaken = "UNIQUE constraint failed: Users.Name";
        public const string EmailTaken = "UNIQUE constraint failed: Users.Email";

        public const string CheckEmail = "CHECK constraint failed: CK_Users_Email";
        public const string CheckBirthday = "CHECK constraint failed: CK_Users_Birthday";
        public const string CheckAgeRestriction = "CHECK constraint failed: CK_Users_Birthday";
        public const string CheckHeight = "CHECK constraint failed: CK_Users_Height";
        public const string CheckWeight = "CHECK constraint failed: CK_Users_Weight";
    }

    public static class Workouts
    {
        public const string NameTaken = "UNIQUE constraint failed: Workouts.UserId, Workouts.Name";

        public const string CheckDuration = "CHECK constraint failed: CK_Workouts_DurationMinutes";
        public const string CheckStartedAt = "CHECK constraint failed: CK_Workouts_StartedAt";

        public const string TriggerBeforeRegistration = "'Workout start date can''t precede registration date'";
    }

    public static class Exercises
    {
        public const string NameTaken = "UNIQUE constraint failed: Exercises.Name";
    }

    public static class WorkoutExercises
    {
        public const string WorkoutExerciseTaken = "UNIQUE constraint failed: WorkoutExercises.WorkoutId, WorkoutExercises.ExerciseId";

        public const string CheckSets = "CHECK constraint failed: CK_WorkoutExercises_Sets";
        public const string CheckReps = "CHECK constraint failed: CK_WorkoutExercises_Reps";
        public const string CheckWeightUsed = "CHECK constraint failed: CK_WorkoutExercises_WeightUsed";
    }

    public static class Goals
    {
        public const string CheckTargetWeight = "CHECK constraint failed: CK_Goals_TargetWeight";
        public const string CheckDates = "CHECK constraint failed: CK_Goals_StartDate_EndDate";

        public const string TriggerBeforeRegistration = "'Goal start date can''t precede registration date'";
        public const string TriggerValidateWeight = "'Weight target conflicts with goal type'";
        public const string TriggerValidateOverlapping = "'Same type user goals can''t overlap'";
    }

    public static class FoodItems
    {
        public const string NameTaken = "UNIQUE constraint failed: FoodItems.Name";

        public const string CheckCalories = "CHECK constraint failed: CK_FoodItems_Calories";
        public const string CheckProtein = "CHECK constraint failed: CK_FoodItems_Protein";
        public const string CheckCarbs = "CHECK constraint failed: CK_FoodItems_Carbs";
        public const string CheckFat = "CHECK constraint failed: CK_FoodItems_Fat";
    }

    public static class FoodLogs
    {
        public const string CheckDate = "CHECK constraint failed: CK_FoodLogs_Date";
        public const string CheckServings = "CHECK constraint failed: CK_FoodLogs_Servings";
        public const string CheckQuantity = "CHECK constraint failed: CK_FoodLogs_Quantity";

        public const string TriggerBeforeRegistration = "'Food log date can''t precede registration date'";
    }

    public static class ProgressLogs
    {
        public const string DuplicateDate = "UNIQUE constraint failed: ProgressLogs.UserId, ProgressLogs.Date";

        public const string CheckDate = "CHECK constraint failed: CK_ProgressLogs_Date";
        public const string CheckWeight = "CHECK constraint failed: CK_ProgressLogs_Weight";
        public const string CheckBodyFat = "CHECK constraint failed: CK_ProgressLogs_BodyFat";
        public const string CheckWaistCm = "CHECK constraint failed: CK_ProgressLogs_WaistCm";
        public const string CheckChestCm = "CHECK constraint failed: CK_ProgressLogs_ChestCm";
        public const string CheckArmsCm = "CHECK constraint failed: CK_ProgressLogs_ArmsCm";

        public const string TriggerBeforeRegistration = "'Progress log date can''t precede registration date'";
        public const string TriggerWeightChangeLimit = "'Weight change exceeds allowed limit'";
    }
}
