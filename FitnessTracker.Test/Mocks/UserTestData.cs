using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.Domain.Constants;

namespace FitnessTracker.Test.Mocks;

public static class UserTestData
{
    public static IEnumerable<object[]> InvalidRegisterRequests() =>
    [
        [RegisterRequests.NoName, nameof(RegisterRequest.Name), new[] { ValidationErrors.NameRequired }],

        [RegisterRequests.NameTooLong, nameof(RegisterRequest.Name), new[] { ValidationErrors.InvalidNameLength }],

        [RegisterRequests.NoEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.EmailRequired, ValidationErrors.InvalidEmail }],

        [RegisterRequests.InvalidEmail, nameof(RegisterRequest.Email), new[] { ValidationErrors.InvalidEmail }],

        [RegisterRequests.EmailTooLong, nameof(RegisterRequest.Email), new[] { ValidationErrors.InvalidEmailLength }],

        [RegisterRequests.NoPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.PasswordRequired }],

        [RegisterRequests.InvalidPassword, nameof(RegisterRequest.Password), new[] { ValidationErrors.InvalidPassword }],

        [RegisterRequests.NoConfirmedPassword, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.ConfirmPassword, ValidationErrors.PasswordsMismatch }],

        [RegisterRequests.NonMatchingPasswords, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationErrors.PasswordsMismatch }],

        [RegisterRequests.NoPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.PhoneRequired, ValidationErrors.InvalidPhone }],

        [RegisterRequests.InvalidPhone, nameof(RegisterRequest.Phone), new[] { ValidationErrors.InvalidPhone }],

        [RegisterRequests.PhoneTooLong, nameof(RegisterRequest.Phone), new[] { ValidationErrors.InvalidPhoneLength }],

        [RegisterRequests.NoBirthday, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.BirthdayRequired, "Invalid data value" }],

        [RegisterRequests.BirthdayFuture, nameof(RegisterRequest.Birthday), new[] { ValidationErrors.InvalidBirthday }],

        [RegisterRequests.NoHeight, nameof(RegisterRequest.Height), new[] { ValidationErrors.HeightRequired }],

        [RegisterRequests.HeightTooLow, nameof(RegisterRequest.Height), new[] { ValidationErrors.InvalidHeight }],

        [RegisterRequests.HeightTooHigh, nameof(RegisterRequest.Height), new[] { ValidationErrors.InvalidHeight }],

        [RegisterRequests.NoWeight, nameof(RegisterRequest.Weight), new[] { ValidationErrors.WeightRequired }],

        [RegisterRequests.WeightTooLow, nameof(RegisterRequest.Weight), new[] { ValidationErrors.InvalidWeight }],

        [RegisterRequests.WeightTooHigh, nameof(RegisterRequest.Weight), new[] { ValidationErrors.InvalidWeight }],

        [RegisterRequests.NoGender, nameof(RegisterRequest.Gender), new[] { ValidationErrors.GenderRequired }]
    ];

    public static IEnumerable<object[]> InvalidLoginRequests() =>
    [
        [LoginRequests.NoEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.EmailRequired, ValidationErrors.InvalidEmail }],

        [LoginRequests.InvalidEmail, nameof(LoginRequest.Email), new[] { ValidationErrors.InvalidEmail }],

        [LoginRequests.EmailTooLong, nameof(LoginRequest.Email), new[] { ValidationErrors.InvalidEmailLength }],

        [LoginRequests.NoPassword, nameof(LoginRequest.Password), new[] { ValidationErrors.PasswordRequired }]
    ];

    public static IEnumerable<object[]> InvalidAddUsers() =>
    [
        [AddUsers.UserInvalidEmail(), ConstraintMessages.CheckUsersEmail],
        [AddUsers.UserFutureBirthday(), ConstraintMessages.CheckUsersBirthday],
        [AddUsers.UserInvalidHeight(), ConstraintMessages.CheckUsersHeight],
        [AddUsers.UserInvalidWeight(), ConstraintMessages.CheckUsersWeight],
        [AddUsers.UserDuplicatedName("John Doe"), ConstraintMessages.DuplicatedName],
        [AddUsers.UserDuplicatedEmail("john.doe@email.com"), ConstraintMessages.DuplicatedEmail]
    ];

    public static IEnumerable<object[]> DuplicatedFieldRegisterRequests() =>
    [
        [RegisterRequests.DuplicatedName, ErrorMessages.DuplicatedName, ErrorMessages.NameTaken],
        [RegisterRequests.DuplicatedEmail, ErrorMessages.DuplicatedEmail, ErrorMessages.EmailTaken]
    ];

    public static IEnumerable<object[]> DuplicatedFieldRegisterRequestsService() =>
    [
        [RegisterRequests.DuplicatedName, ErrorMessages.NameTaken],
        [RegisterRequests.DuplicatedEmail, ErrorMessages.EmailTaken]
    ];
}
