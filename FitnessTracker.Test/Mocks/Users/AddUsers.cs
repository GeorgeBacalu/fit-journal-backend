using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Test.Mocks.Users;

public static class AddUsers
{
    public static User NewUser() => new()
    {
        Id = Guid.Parse("36712aa4-77f0-4510-8425-cf53dad54840"),
        Name = "Michael Johnson",
        Email = "michael.johnson@email.com",
        PasswordHash = "$2a$11$j.sHfk5pNbgqfwzFTwZqEeeyD1rbI9XJPcNW9chybMVNNhOjuLfhG",
        Phone = "+61-402-555-0836",
        Birthday = new(1990, 1, 3),
        Height = 178.0,
        Weight = 79.0,
        Gender = Gender.Male,
        Role = Role.User,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = null,
        DeletedAt = null
    };

    private static User With(Action<User> mutate)
    {
        var user = NewUser();
        mutate(user);
        return user;
    }

    public static User UserInvalidEmail() =>
        With(user => user.Email = "michael.johnson");

    public static User UserFutureBirthday() =>
        With(user => user.Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)));

    public static User UserInvalidHeight() =>
        With(user => user.Height = -1);

    public static User UserInvalidWeight() =>
        With(user => user.Weight = -1);

    public static User UserDuplicatedName(string existingName) =>
        With(user => user.Name = existingName);

    public static User UserDuplicatedEmail(string existingEmail) =>
        With(user => user.Email = existingEmail);
}
