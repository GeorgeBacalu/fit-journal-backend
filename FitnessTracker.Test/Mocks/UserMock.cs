using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Test.Mocks;

public static class UserMock
{
    public static readonly List<User> Users = [new()
    {
        Id = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e"),
        Name = "John Doe",
        Email = "john.doe@email.com",
        PasswordHash = "$2a$11$keNH6EuCs3ZNsWXSyN0dYuOwBgDDVBJ3tkEois5anX1K3bm3xCeFq", // JohnDoePassword0!
        Phone = "+1202-555-0125",
        Birthday = new DateOnly(1990, 5, 21),
        Height = 180.0,
        Weight = 82.5,
        Gender = Gender.Male,
        Role = Role.User,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = null,
        DeletedAt = null
    }, new() {
        Id = Guid.Parse("7eb88892-549b-4cae-90be-c52088354643"),
        Name = "Jane Smith",
        Email = "jane.smith@email.com",
        PasswordHash = "$2a$11$odL3DYVYeOnmMCUzGtVRduW625Zf007zv4E2SP04uiCPmazcrU0he", // JaneSmithPassword0!
        Phone = "+44-7700-900123",
        Birthday = new DateOnly(1992, 11, 3),
        Height = 165.0,
        Weight = 63.2,
        Gender = Gender.Female,
        Role = Role.User,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = null,
        DeletedAt = null
    }];

    public static readonly User NewUser = new()
    {
        Id = Guid.Parse("36712aa4-77f0-4510-8425-cf53dad54840"),
        Name = "Michael Johnson",
        Email = "michael.johnson@email.com",
        PasswordHash = "$2a$11$j.sHfk5pNbgqfwzFTwZqEeeyD1rbI9XJPcNW9chybMVNNhOjuLfhG", // MichaelJohnsonPassword0!
        Phone = "+61-402-555-0836",
        Birthday = new DateOnly(1988, 2, 14),
        Height = 178.0,
        Weight = 79.1,
        Gender = Gender.Male,
        Role = Role.User,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = null,
        DeletedAt = null
    };

    public static readonly RegisterRequest RegisterRequest = new()
    {
        Name = "Michael Johnson",
        Email = "michael.johnson@email.com",
        Password = "MichaelJohnsonPassword0!",
        ConfirmedPassword = "MichaelJohnsonPassword0!",
        Phone = "+61-402-555-0836",
        Birthday = new DateOnly(1988, 2, 14),
        Height = 178.0,
        Weight = 79.1,
        Gender = Gender.Male
    };

    public static readonly RegisterRequest RegisterRequestNoName = RegisterRequest with { Name = "" };
    
    public static readonly RegisterRequest RegisterRequestNameTooLong = RegisterRequest with { Name = "Michael Jonathan Alexander Christopher Johnson Senior the Third" };
    
    public static readonly RegisterRequest RegisterRequestNoEmail = RegisterRequest with { Email = "" };
    
    public static readonly RegisterRequest RegisterRequestInvalidEmail = RegisterRequest with { Email = "michael.johnson" };
    
    public static readonly RegisterRequest RegisterRequestEmailTooLong = RegisterRequest with { Email = "michael.jonathan.alexander.christopher.johnson.senior.the.third@email.com" };
    
    public static readonly RegisterRequest RegisterRequestNoPassword = RegisterRequest with { Password = "" };
    
    public static readonly RegisterRequest RegisterRequestInvalidPassword = RegisterRequest with { Password = "MichaelJohnsonPassword0", ConfirmedPassword = "MichaelJohnsonPassword0" };
    
    public static readonly RegisterRequest RegisterRequestNoConfirmedPassword = RegisterRequest with { ConfirmedPassword = "" };
    
    public static readonly RegisterRequest RegisterRequestNonMatchingPasswords = RegisterRequest with { Password = "MichaelJohnsonPassword0!", ConfirmedPassword = "MichaelJohnsonPassword1!" };
    
    public static readonly RegisterRequest RegisterRequestNoPhone = RegisterRequest with { Phone = "" };
    
    public static readonly RegisterRequest RegisterRequestInvalidPhone = RegisterRequest with { Phone = "invalid-phone" };
    
    public static readonly RegisterRequest RegisterRequestPhoneTooLong = RegisterRequest with { Phone = "+61-402-555-0836-9999" };
    
    public static readonly RegisterRequest RegisterRequestNoBirthday = RegisterRequest with { Birthday = default };
    
    public static readonly RegisterRequest RegisterRequestBirthdayFuture = RegisterRequest with { Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) };
    
    public static readonly RegisterRequest RegisterRequestNoHeight = RegisterRequest with { Height = default };
    
    public static readonly RegisterRequest RegisterRequestHeightTooLow = RegisterRequest with { Height = -1 };
    
    public static readonly RegisterRequest RegisterRequestHeightTooHigh = RegisterRequest with { Height = 300 };
    
    public static readonly RegisterRequest RegisterRequestNoWeight = RegisterRequest with { Weight = default };
    
    public static readonly RegisterRequest RegisterRequestWeightTooLow = RegisterRequest with { Weight = -1 };
    
    public static readonly RegisterRequest RegisterRequestWeightTooHigh = RegisterRequest with { Weight = 300 };
    
    public static readonly RegisterRequest RegisterRequestNoGender = RegisterRequest with { Gender = default };
    
    public static readonly RegisterRequest RegisterRequestDuplicatedName = RegisterRequest with { Name = "John Doe" };
    
    public static readonly RegisterRequest RegisterRequestDuplicatedEmail = RegisterRequest with { Email = "john.doe@email.com" };
    
    public static readonly RegisterRequest RegisterRequestUnder13 = RegisterRequest with { Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-13).AddDays(1)) };

    public static IEnumerable<object[]> InvalidRegisterRequests() =>
    [
        [RegisterRequestNoName, nameof(RegisterRequest.Name), new[] { ValidationConstants.NameRequired }],
        [RegisterRequestNameTooLong, nameof(RegisterRequest.Name), new[] { ValidationConstants.InvalidNameLength }],
        [RegisterRequestNoEmail, nameof(RegisterRequest.Email), new[] { ValidationConstants.EmailRequired, ValidationConstants.InvalidEmail }],
        [RegisterRequestInvalidEmail, nameof(RegisterRequest.Email), new[] { ValidationConstants.InvalidEmail }],
        [RegisterRequestEmailTooLong, nameof(RegisterRequest.Email), new[] { ValidationConstants.InvalidEmailLength }],
        [RegisterRequestNoPassword, nameof(RegisterRequest.Password), new[] { ValidationConstants.PasswordRequired }],
        [RegisterRequestInvalidPassword, nameof(RegisterRequest.Password), new[] { ValidationConstants.InvalidPassword }],
        [RegisterRequestNoConfirmedPassword, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationConstants.ConfirmPassword, ValidationConstants.PasswordsMismatch }],
        [RegisterRequestNonMatchingPasswords, nameof(RegisterRequest.ConfirmedPassword), new[] { ValidationConstants.PasswordsMismatch }],
        [RegisterRequestNoPhone, nameof(RegisterRequest.Phone), new[] { ValidationConstants.PhoneRequired, ValidationConstants.InvalidPhone }],
        [RegisterRequestInvalidPhone, nameof(RegisterRequest.Phone), new[] { ValidationConstants.InvalidPhone }],
        [RegisterRequestPhoneTooLong, nameof(RegisterRequest.Phone), new[] { ValidationConstants.InvalidPhoneLength }],
        [RegisterRequestNoBirthday, nameof(RegisterRequest.Birthday), new[] { ValidationConstants.BirthdayRequired, "Invalid data value" }],
        [RegisterRequestBirthdayFuture, nameof(RegisterRequest.Birthday), new[] { ValidationConstants.InvalidBirthday }],
        [RegisterRequestNoHeight, nameof(RegisterRequest.Height), new[] { ValidationConstants.HeightRequired }],
        [RegisterRequestHeightTooLow, nameof(RegisterRequest.Height), new[] { ValidationConstants.InvalidHeight }],
        [RegisterRequestHeightTooHigh, nameof(RegisterRequest.Height), new[] { ValidationConstants.InvalidHeight }],
        [RegisterRequestNoWeight, nameof(RegisterRequest.Weight), new[] { ValidationConstants.WeightRequired }],
        [RegisterRequestWeightTooLow, nameof(RegisterRequest.Weight), new[] { ValidationConstants.InvalidWeight }],
        [RegisterRequestWeightTooHigh, nameof(RegisterRequest.Weight), new[] { ValidationConstants.InvalidWeight }],
        [RegisterRequestNoGender, nameof(RegisterRequest.Gender), new[] { ValidationConstants.GenderRequired }]
    ];

    public static IEnumerable<object[]> DuplicatedFieldRegisterRequests() =>
    [
        [RegisterRequestDuplicatedName, ErrorMessageConstants.DuplicatedName, ErrorMessageConstants.NameTaken],
        [RegisterRequestDuplicatedEmail, ErrorMessageConstants.DuplicatedEmail, ErrorMessageConstants.EmailTaken]
    ];
    
    public static IEnumerable<object[]> DuplicatedFieldRegisterRequestsService() =>
    [
        [RegisterRequestDuplicatedName, ErrorMessageConstants.NameTaken],
        [RegisterRequestDuplicatedEmail, ErrorMessageConstants.EmailTaken]
    ];
}
