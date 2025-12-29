namespace FitnessTracker.Core.Constants;

public static class ValidationRules
{
    public static class Users
    {
        public const string PasswordRegex = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])\S{6,}$";
        public const string PhoneRegex = @"^\+?[0-9\s\-()]+$";
    }
}
