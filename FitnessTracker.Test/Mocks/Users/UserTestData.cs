using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Test.Mocks.Auth;

namespace FitnessTracker.Test.Mocks.Users;

public static class UserTestData
{
    public static IEnumerable<object[]> InvalidRegisterRequests() =>
    [
        [RegisterRequests.NoName, nameof(RegisterRequest.Name), new[] { ValidationErrors.Common.NameRequired }],

        [RegisterRequests.NameTooLong, nameof(RegisterRequest.Name), new[] { ValidationErrors.Common.InvalidNameLength }],

        [RegisterRequests.NoEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.EmailRequired, ValidationErrors.Users.InvalidEmail }],

        [RegisterRequests.InvalidEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.InvalidEmail }],

        [RegisterRequests.EmailTooLong, nameof(RegisterRequest.Email), new[] { ValidationErrors.Users.InvalidEmailLength }],

        [RegisterRequests.NoPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.Users.PasswordRequired }],

        [RegisterRequests.InvalidPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.Users.InvalidPassword }],

        [RegisterRequests.NoConfirmedPassword, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.Users.ConfirmPassword, ValidationErrors.Users.PasswordsMismatch }],

        [RegisterRequests.NonMatchingPasswords, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.Users.PasswordsMismatch }],

        [RegisterRequests.NoPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.PhoneRequired, ValidationErrors.Users.InvalidPhone }],

        [RegisterRequests.InvalidPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.InvalidPhone }],

        [RegisterRequests.PhoneTooLong, nameof(RegisterRequest.Phone), new[] { ValidationErrors.Users.InvalidPhoneLength }],

        [RegisterRequests.NoBirthday, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.Users.BirthdayRequired }],

        [RegisterRequests.BirthdayFuture, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.Users.InvalidBirthday }],

        [RegisterRequests.NoHeight, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.HeightRequired }],

        [RegisterRequests.HeightTooLow, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.InvalidHeight }],

        [RegisterRequests.HeightTooHigh, nameof(RegisterRequest.Height), new[] { ValidationErrors.Users.InvalidHeight }],

        [RegisterRequests.NoWeight, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.WeightRequired }],

        [RegisterRequests.WeightTooLow, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.InvalidWeight }],

        [RegisterRequests.WeightTooHigh, nameof(RegisterRequest.Weight), new[] { ValidationErrors.Users.InvalidWeight }],

        [RegisterRequests.NoGender, nameof(RegisterRequest.Gender), new[] { ValidationErrors.Users.GenderRequired }]
    ];

    public static IEnumerable<object[]> InvalidLoginRequests() =>
    [
        [LoginRequests.NoEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.EmailRequired, ValidationErrors.Users.InvalidEmail }],

        [LoginRequests.InvalidEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.InvalidEmail }],

        [LoginRequests.EmailTooLong, nameof(LoginRequest.Email), new[] { ValidationErrors.Users.InvalidEmailLength }],

        [LoginRequests.NoPassword, nameof(LoginRequest.Password), new[] { ValidationErrors.Users.PasswordRequired }]
    ];

    public static IEnumerable<object[]> InvalidAddUsers() =>
    [
        [AddUsers.UserInvalidEmail(), DatabaseErrors.Users.CheckUsersEmail],
        [AddUsers.UserFutureBirthday(), DatabaseErrors.Users.CheckUsersBirthday],
        [AddUsers.UserInvalidHeight(), DatabaseErrors.Users.CheckUsersHeight],
        [AddUsers.UserInvalidWeight(), DatabaseErrors.Users.CheckUsersWeight],
        [AddUsers.UserDuplicatedName("John Doe"), DatabaseErrors.Users.DuplicatedName],
        [AddUsers.UserDuplicatedEmail("john.doe@email.com"), DatabaseErrors.Users.DuplicatedEmail]
    ];

    public static IEnumerable<object[]> DuplicatedFieldRegisterRequests() =>
    [
        [RegisterRequests.DuplicatedName, ErrorMessages.Users.NameTaken],
        [RegisterRequests.DuplicatedEmail, ErrorMessages.Users.EmailTaken]
    ];
}
