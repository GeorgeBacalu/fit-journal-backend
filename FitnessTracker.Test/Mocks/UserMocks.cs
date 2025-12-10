using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Test.Mocks;

public static class UserMocks
{
    public static readonly List<User> Users =
    [
        new()
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
        },
        new()
        {
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
        }
    ];

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
}
