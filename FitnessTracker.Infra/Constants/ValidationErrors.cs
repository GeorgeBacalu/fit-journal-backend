namespace FitnessTracker.Infra.Constants;

public static class ValidationErrors
{
    // Auth

    public const string NameRequired = "Name is required";
    public const string EmailRequired = "Email is required";
    public const string PasswordRequired = "Password is required";
    public const string ConfirmPassword = "Confirm your password";
    public const string PhoneRequired = "Phone is required";
    public const string BirthdayRequired = "Birthday is required";
    public const string HeightRequired = "Height is required";
    public const string WeightRequired = "Weight is required";
    public const string GenderRequired = "Gender is required";

    public const string InvalidNameLength = "Invalid name length";
    public const string InvalidEmail = "Invalid email address";
    public const string InvalidEmailLength = "Invalid email length";
    public const string InvalidPassword = "Invalid password";
    public const string PasswordsMismatch = "Passwords don't match";
    public const string InvalidPhone = "Invalid phone";
    public const string InvalidPhoneLength = "Invalid phone length";
    public const string InvalidBirthday = "Birthday can't be in the future";
    public const string InvalidHeight = "Invalid height";
    public const string InvalidWeight = "Invalid weight";

    // Description

    public const string InvalidDescriptionLength = "Invalid description length";
    public const string InvalidNotesLength = "Invalid notes length";
    public const string InvalidDuration = "Invalid duration";
    public const string InvalidWorkoutDate = "Workout date can't be in the future";
    public const string UserIdRequired = "User ID is required";
}
