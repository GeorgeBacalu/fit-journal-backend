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

        [RegisterRequests.NameTooLong, nameof(RegisterRequest.Name), new[] { ValidationErrors.Common.NameTooLong.Message.Replace("{MaxLength}", "50") }],

        [RegisterRequests.NoEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.EmailRequired.Message, ValidationErrors.Users.InvalidEmail.Message }],

        [RegisterRequests.InvalidEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.InvalidEmail.Message }],

        [RegisterRequests.EmailTooLong, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.EmailTooLong.Message.Replace("{MaxLength}", "50") }],

        [RegisterRequests.NoPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.Users.PasswordRequired.Message, ValidationErrors.Users.InvalidPassword.Message, ValidationErrors.Users.InvalidPasswordLength.Message.Replace("{MinLength}", "6").Replace("{MaxLength}", "30") }],

        [RegisterRequests.InvalidPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.Users.InvalidPassword.Message }],

        [RegisterRequests.NoConfirmedPassword, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.Users.ConfirmPassword.Message, ValidationErrors.Users.PasswordsMismatch.Message }],

        [RegisterRequests.NonMatchingPasswords, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.Users.PasswordsMismatch.Message }],

        [RegisterRequests.NoPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.PhoneRequired.Message, ValidationErrors.Users.InvalidPhone.Message }],

        [RegisterRequests.InvalidPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.InvalidPhone.Message }],

        [RegisterRequests.PhoneTooLong, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.PhoneTooLong.Message.Replace("{MaxLength}", "20") }],

        [RegisterRequests.NoBirthday, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.Users.BirthdayRequired.Message }],

        [RegisterRequests.BirthdayFuture, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.Users.FutureBirthday.Message, ValidationErrors.Users.AgeRestriction.Message }],

        [RegisterRequests.NoHeight, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.HeightRequired.Message, ValidationErrors.Users.HeightOutOfRange.Message }],

        [RegisterRequests.HeightTooLow, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.HeightOutOfRange.Message }],

        [RegisterRequests.HeightTooHigh, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.HeightOutOfRange.Message }],

        [RegisterRequests.NoWeight, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.WeightRequired.Message, ValidationErrors.Users.WeightOutOfRange.Message }],

        [RegisterRequests.WeightTooLow, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.WeightOutOfRange.Message }],

        [RegisterRequests.WeightTooHigh, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.WeightOutOfRange.Message }],

        [RegisterRequests.NoGender, nameof(RegisterRequest.Gender), new[] { ValidationErrors.Users.GenderRequired.Message }]
    ];

    public static IEnumerable<object[]> InvalidLoginRequests() =>
    [
        [LoginRequests.NoEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.EmailRequired.Message, ValidationErrors.Users.InvalidEmail.Message }],

        [LoginRequests.InvalidEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.InvalidEmail.Message }],

        [LoginRequests.EmailTooLong, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.EmailTooLong.Message.Replace("{MaxLength}", "50") }],

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
