namespace FitnessTracker.Infra.Constants;

public static class ValidationRules
{
    public const string PasswordRegex = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])\S{6,}$";
}
