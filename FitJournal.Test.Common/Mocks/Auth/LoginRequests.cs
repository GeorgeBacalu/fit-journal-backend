using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Test.Common.Constants;

namespace FitJournal.Test.Common.Mocks.Auth;

public static class LoginRequests
{
    public static readonly LoginRequest Valid = new()
    {
        Email = "john.doe@email.com",
        Password = "JohnDoePassword0!"
    };

    public static readonly LoginRequest NoEmail = Valid with { Email = string.Empty };

    public static readonly LoginRequest NonExistingEmail = Valid with { Email = ValidationSamples.Users.NonExistingEmail };

    public static readonly LoginRequest InvalidEmail = Valid with { Email = ValidationSamples.Users.InvalidEmail };

    public static readonly LoginRequest EmailTooLong = Valid with { Email = ValidationSamples.Users.EmailTooLong };

    public static readonly LoginRequest NoPassword = Valid with { Password = string.Empty };

    public static readonly LoginRequest WrongPassword = Valid with { Password = "JohnDoePassword1!" };
}
