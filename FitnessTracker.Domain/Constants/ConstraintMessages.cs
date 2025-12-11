namespace FitnessTracker.Domain.Constants;

public static class ConstraintMessages
{
    public const string DuplicatedName = "UNIQUE constraint failed: Users.Name";
    public const string DuplicatedEmail = "UNIQUE constraint failed: Users.Email";
    public const string CheckUsersEmail = "CHECK constraint failed: CK_Users_Email";
    public const string CheckUsersBirthday = "CHECK constraint failed: CK_Users_Birthday";
    public const string CheckUsersHeight = "CHECK constraint failed: CK_Users_Height";
    public const string CheckUsersWeight = "CHECK constraint failed: CK_Users_Weight";
}
