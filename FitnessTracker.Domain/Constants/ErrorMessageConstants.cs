namespace FitnessTracker.Domain.Constants;

public static class ErrorMessageConstants
{
    public const string DuplicatedName = "Duplicated name";
    public const string DuplicatedEmail = "Duplicated email";
    public const string NameTaken = "Name already taken";
    public const string EmailTaken = "Email already taken";

    public const string AgeRestriction = "You must be at least 13 years old";

    public const string UserEmailNotFound = "User with email {0} not found";
    public const string InvalidCredentials = "Invalid credentials";
}
