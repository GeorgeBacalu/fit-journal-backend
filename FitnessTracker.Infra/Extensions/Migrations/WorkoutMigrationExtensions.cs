using FitnessTracker.Infra.Constants;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

internal static class WorkoutMigrationExtensions
{
    internal static OperationBuilder<SqlOperation> AddWorkoutBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.WorkoutsBeforeRegistration} ON [dbo].[Workouts]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT 1 FROM inserted i
                JOIN [dbo].[Users] u ON u.[Id] = i.[UserId]
                WHERE i.[DeletedAt] IS NULL AND u.[DeletedAt] IS NULL AND i.[StartedAt] < u.[CreatedAt]
            ) THROW 50001, {DbErrors.Workouts.TriggerBeforeRegistration}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> DropWorkoutBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.WorkoutsBeforeRegistration}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.WorkoutsBeforeRegistration};");
}
