using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

public static class WorkoutMigrationExtensions
{
    public static OperationBuilder<SqlOperation> AddWorkoutStartDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        CREATE TRIGGER TR_Workouts_BeforeUserRegistration ON [Workouts]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN Users u ON u.[Id] = i.[UserId]
                WHERE i.[StartedAt] < u.[CreatedAt]
            )
            BEGIN
                THROW 50001, 'Workout start date can''t be before user registration date', 1;
            END
        END;");

    public static OperationBuilder<SqlOperation> DropWorkoutStartDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        IF OBJECT_ID('TR_Workouts_BeforeUserRegistration', 'TR') IS NOT NULL
        DROP TRIGGER TR_Workouts_BeforeUserRegistration;");
}
