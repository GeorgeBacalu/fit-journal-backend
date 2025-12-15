using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Test.Mocks.Users;

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
            Birthday = new(1990, 1, 1),
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
            Birthday = new(1990, 1, 2),
            Height = 165.0,
            Weight = 63.5,
            Gender = Gender.Female,
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            DeletedAt = null
        }
    ];
}
