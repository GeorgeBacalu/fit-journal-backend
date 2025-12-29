using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Test.Common.Mocks.Auth;

public static class RegisterRequests
{
    public static readonly RegisterRequest Valid = new()
    {
        Name = "Michael Johnson",
        Email = "michael.johnson@email.com",
        Password = "MichaelJohnsonPassword0!",
        ConfirmedPassword = "MichaelJohnsonPassword0!",
        Phone = "+61-402-555-0836",
        Birthday = new DateOnly(1988, 2, 14),
        Height = 178m,
        Weight = 79m,
        Gender = Gender.Male
    };

    public static readonly RegisterRequest NoName = Valid with { Name = string.Empty };

    public static readonly RegisterRequest NameTooLong = Valid with { Name = "Michael Jonathan Alexander Christopher Johnson Senior the Third" };

    public static readonly RegisterRequest NoEmail = Valid with { Email = string.Empty };

    public static readonly RegisterRequest InvalidEmail = Valid with { Email = "michael.johnson" };

    public static readonly RegisterRequest EmailTooLong = Valid with { Email = "michael.jonathan.alexander.christopher.johnson.senior.the.third@email.com" };

    public static readonly RegisterRequest NoPassword = Valid with { Password = string.Empty };

    public static readonly RegisterRequest InvalidPassword = Valid with { Password = "MichaelJohnsonPassword0", ConfirmedPassword = "MichaelJohnsonPassword0" };

    public static readonly RegisterRequest NoConfirmedPassword = Valid with { ConfirmedPassword = string.Empty };

    public static readonly RegisterRequest NonMatchingPasswords = Valid with { Password = "MichaelJohnsonPassword0!", ConfirmedPassword = "MichaelJohnsonPassword1!" };

    public static readonly RegisterRequest NoPhone = Valid with { Phone = string.Empty };

    public static readonly RegisterRequest InvalidPhone = Valid with { Phone = "invalid-phone" };

    public static readonly RegisterRequest PhoneTooLong = Valid with { Phone = "+61-402-555-0836-9999" };

    public static readonly RegisterRequest NoBirthday = Valid with { Birthday = default };

    public static readonly RegisterRequest BirthdayFuture = Valid with { Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) };

    public static readonly RegisterRequest NoHeight = Valid with { Height = default };

    public static readonly RegisterRequest HeightTooLow = Valid with { Height = -1 };

    public static readonly RegisterRequest HeightTooHigh = Valid with { Height = 300 };

    public static readonly RegisterRequest NoWeight = Valid with { Weight = default };

    public static readonly RegisterRequest WeightTooLow = Valid with { Weight = -1 };

    public static readonly RegisterRequest WeightTooHigh = Valid with { Weight = 300 };

    public static readonly RegisterRequest NoGender = Valid with { Gender = default };

    public static readonly RegisterRequest DuplicatedName = Valid with { Name = "John Doe" };

    public static readonly RegisterRequest DuplicatedEmail = Valid with { Email = "john.doe@email.com" };

    public static readonly RegisterRequest Under13 = Valid with { Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-13).AddDays(1)) };
}
