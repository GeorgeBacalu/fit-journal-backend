using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Infra.Constants;
using FitJournal.Test.Common.Mocks.Auth;

namespace FitJournal.Test.Common.Mocks.Users;

public static class UserTestData
{
    public static IEnumerable<object[]> InvalidRegisterRequests() =>
    [
        [RegisterRequests.NoName, nameof(RegisterRequest.Name), new[] { ValidationErrors.Common.NameRequired.Message }],

        [RegisterRequests.NameTooLong, nameof(RegisterRequest.Name), new[] { "Name exceeds 50 characters" }],

        [RegisterRequests.NoEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.EmailRequired.Message, ValidationErrors.Users.InvalidEmail.Message }],

        [RegisterRequests.InvalidEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.InvalidEmail.Message }],

        [RegisterRequests.EmailTooLong, nameof(RegisterRequest.Email), new[] { "Email exceeds 50 characters" }],

        [RegisterRequests.NoPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.Users.PasswordRequired.Message }],

        [RegisterRequests.InvalidPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.Users.InvalidPassword.Message }],

        [RegisterRequests.NoConfirmedPassword, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.Users.ConfirmPassword.Message, ValidationErrors.Users.PasswordsMismatch.Message }],

        [RegisterRequests.NonMatchingPasswords, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.Users.PasswordsMismatch.Message }],

        [RegisterRequests.NoPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.PhoneRequired.Message, ValidationErrors.Users.InvalidPhone.Message }],

        [RegisterRequests.InvalidPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.InvalidPhone.Message }],

        [RegisterRequests.PhoneTooLong, nameof(RegisterRequest.Phone), new[] { "Phone exceeds 20 characters" }],

        [RegisterRequests.NoBirthday, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.Users.BirthdayRequired.Message }],

        [RegisterRequests.BirthdayFuture, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.Users.FutureBirthday.Message }],

        [RegisterRequests.NoHeight, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.HeightRequired.Message }],

        [RegisterRequests.HeightTooLow, nameof(RegisterRequest.Height), new[] { "Height must be 120-250 cm" }],

        [RegisterRequests.HeightTooHigh, nameof(RegisterRequest.Height), new[] { "Height must be 120-250 cm" }],

        [RegisterRequests.NoWeight, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.WeightRequired.Message }],

        [RegisterRequests.WeightTooLow, nameof(RegisterRequest.Weight), new[] { "Weight must be 25-250 kg" }],

        [RegisterRequests.WeightTooHigh, nameof(RegisterRequest.Weight), new[] { "Weight must be 25-250 kg" }],

        [RegisterRequests.NoGender, nameof(RegisterRequest.Gender), new[] { ValidationErrors.Users.GenderRequired.Message }]
    ];

    public static IEnumerable<object[]> InvalidLoginRequests() =>
    [
        [LoginRequests.NoEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.EmailRequired.Message, ValidationErrors.Users.InvalidEmail.Message }],

        [LoginRequests.InvalidEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.InvalidEmail.Message }],

        [LoginRequests.EmailTooLong, nameof(LoginRequest.Email), new[] { "Email exceeds 50 characters" }],

        [LoginRequests.NoPassword, nameof(LoginRequest.Password), new[] { ValidationErrors.Users.PasswordRequired.Message }]
    ];

    public static IEnumerable<object[]> InvalidAddUsers() =>
    [
        [AddUsers.UserInvalidEmail(), DbErrors.Users.CheckEmail],
        [AddUsers.UserFutureBirthday(), DbErrors.Users.CheckBirthday],
        [AddUsers.UserInvalidHeight(), DbErrors.Users.CheckHeight],
        [AddUsers.UserInvalidWeight(), DbErrors.Users.CheckWeight],
        [AddUsers.UserDuplicatedName("John Doe"), DbErrors.Users.NameTaken],
        [AddUsers.UserDuplicatedEmail("john.doe@email.com"), DbErrors.Users.EmailTaken]
    ];

    public static IEnumerable<object[]> DuplicatedFieldRegisterRequests() =>
    [
        [RegisterRequests.DuplicatedName, ValidationErrors.Common.NameTaken],
        [RegisterRequests.DuplicatedEmail, ValidationErrors.Users.EmailTaken]
    ];
}
