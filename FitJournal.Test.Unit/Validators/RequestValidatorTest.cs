using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Domain.Enums.Goals;
using FitJournal.Domain.Enums.Users;
using FluentAssertions;

namespace FitJournal.Test.Unit.Validators;

public class RequestValidatorTest
{
    [Fact]
    public void RegisterValidator_AcceptsValidRegistration()
    {
        var request = new RegisterRequest
        {
            Name = "Valid User", Email = "valid@fitjournal.test", Password = "Password1!",
            ConfirmedPassword = "Password1!", Phone = "+40700111222",
            Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
            Height = 180, Weight = 80, Gender = Gender.Male
        };

        new RegisterValidator().Validate(request).IsValid.Should().BeTrue();
    }

    [Fact]
    public void RegisterValidator_RejectsInvalidProfileFields()
    {
        var request = new RegisterRequest
        {
            Name = "", Email = "not-an-email", Password = "weak", ConfirmedPassword = "different",
            Phone = "invalid", Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
            Height = 20, Weight = 500, Gender = Gender.Unknown
        };

        var result = new RegisterValidator().Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(x => x.PropertyName).Should().Contain([
            nameof(request.Name), nameof(request.Email), nameof(request.Password),
            nameof(request.ConfirmedPassword), nameof(request.Phone), nameof(request.Birthday),
            nameof(request.Height), nameof(request.Weight), nameof(request.Gender)
        ]);
    }

    [Fact]
    public void LoginValidator_RejectsMissingCredentials()
    {
        var result = new LoginValidator().Validate(new LoginRequest { Email = "", Password = "" });
        result.Errors.Select(x => x.PropertyName).Should().Contain(["Email", "Password"]);
    }

    [Fact]
    public void PasswordValidators_RejectMissingAndMismatchedValues()
    {
        new ChangePasswordValidator().Validate(new ChangePasswordRequest
        {
            CurrentPassword = "", NewPassword = "Password1!", ConfirmedPassword = "Password2!"
        }).IsValid.Should().BeFalse();

        new ResetPasswordValidator().Validate(new ResetPasswordRequest
        {
            Token = "", NewPassword = "Password1!", ConfirmedPassword = "Password2!"
        }).IsValid.Should().BeFalse();
    }

    [Fact]
    public void TokenAndForgotPasswordValidators_RejectEmptyValues()
    {
        new RefreshValidator().Validate(new RefreshRequest { RefreshToken = "" }).IsValid.Should().BeFalse();
        new ForgotPasswordRequestValidator().Validate(new ForgotPasswordRequest { Email = "bad" }).IsValid.Should().BeFalse();
    }

    [Fact]
    public void GoalValidators_AcceptValidRequests()
    {
        var add = new AddGoalRequest
        {
            Name = "Target", Type = GoalType.WeightLoss, TargetWeight = 75,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30))
        };
        new AddGoalValidator().Validate(add).IsValid.Should().BeTrue();
        new EditGoalValidator().Validate(new EditGoalRequest
        {
            Id = Guid.NewGuid(), Name = add.Name, Type = add.Type, TargetWeight = add.TargetWeight,
            StartDate = add.StartDate, EndDate = add.EndDate
        }).IsValid.Should().BeTrue();
    }

    [Fact]
    public void GoalValidators_RejectInvalidDatesWeightAndIds()
    {
        var result = new AddGoalValidator().Validate(new AddGoalRequest
        {
            Name = "", Type = GoalType.Unknown, TargetWeight = 1,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        });
        result.IsValid.Should().BeFalse();
        result.Errors.Select(x => x.PropertyName).Should().Contain(["Name", "Type", "TargetWeight", "StartDate"]);

        new EditGoalValidator().Validate(new EditGoalRequest
        {
            Id = Guid.Empty, Name = "Target", Type = GoalType.WeightLoss, TargetWeight = 75,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow), EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        }).IsValid.Should().BeFalse();
    }

    [Fact]
    public void RemoveGoalsValidator_RejectsEmptyAndDuplicateIds()
    {
        new RemoveGoalsValidator().Validate(new RemoveGoalsRequest()).IsValid.Should().BeFalse();
        var id = Guid.NewGuid();
        new RemoveGoalsValidator().Validate(new RemoveGoalsRequest { Ids = [id, id] }).IsValid.Should().BeFalse();
        new RemoveGoalsValidator().Validate(new RemoveGoalsRequest { Ids = [id] }).IsValid.Should().BeTrue();
    }
}
