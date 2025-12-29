namespace FitnessTracker.Infra.Constants;

public static class DbTriggers
{
    public const string WorkoutsBeforeRegistration = "TR_Workouts_BeforeRegistration";
    
    public const string GoalsBeforeRegistration = "TR_Goals_BeforeRegistration";
    public const string GoalsValidateWeight = "TR_Goals_ValidateWeight";
    public const string GoalsValidateOverlapping = "TR_Goals_ValidateOverlapping";
    
    public const string FoodLogsBeforeRegistration = "TR_FoodLogs_BeforeRegistration";
    
    public const string ProgressLogsBeforeRegistration = "TR_ProgressLogs_BeforeRegistration";
    public const string ProgressLogsWeightChangeLimit = "TR_ProgressLogs_WeightChangeLimit";
}
