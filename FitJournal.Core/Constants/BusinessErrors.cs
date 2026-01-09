using FitJournal.Core.Results;

namespace FitJournal.Core.Constants;

public static class BusinessErrors
{
    public static class Users
    {
        static Error E(string name, string message) => Error.Create(typeof(Users), name, message);

        public static readonly Error InvalidCredentials = E(nameof(InvalidCredentials), "Invalid credentials");
        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"User with id {id} not found");
        public static Error EmailNotFound(string email) => E(nameof(EmailNotFound), $"User with email {email} not found");
    }

    public static class Workouts
    {
        static Error E(string name, string message) => Error.Create(typeof(Workouts), name, message);

        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Workout with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid workout ids found");
    }

    public static class Exercises
    {
        static Error E(string name, string message) => Error.Create(typeof(Exercises), name, message);

        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Exercise with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid exercise ids found");
    }

    public static class WorkoutExercises
    {
        static Error E(string name, string message) => Error.Create(typeof(WorkoutExercises), name, message);

        public static readonly Error UnauthorizedAccess = E(nameof(UnauthorizedAccess), "You can't access another user's workout");
        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Workout exercise with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid workout exercise ids found");
    }

    public static class Goals
    {
        static Error E(string name, string message) => Error.Create(typeof(Goals), name, message);

        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Goal with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid goal ids found");
    }

    public static class FoodItems
    {
        static Error E(string name, string message) => Error.Create(typeof(FoodItems), name, message);

        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Food item with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid food item ids found");
    }

    public static class FoodLogs
    {
        static Error E(string name, string message) => Error.Create(typeof(FoodLogs), name, message);

        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Food log with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid food logs ids found");
    }

    public static class ProgressLogs
    {
        static Error E(string name, string message) => Error.Create(typeof(ProgressLogs), name, message);

        public static Error IdNotFound(Guid id) => E(nameof(IdNotFound), $"Progress log with id {id} not found");
        public static readonly Error IdsNotFound = E(nameof(IdsNotFound), "Invalid progress logs ids found");
    }
}
