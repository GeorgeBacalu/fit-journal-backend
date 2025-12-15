using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions;

public static class WorkoutMigrationExtensions
{
    public static OperationBuilder<SqlOperation> AddWorkoutDateTrigger(this MigrationBuilder migrationBuilder)
        => migrationBuilder.Sql(@"
        CREATE TRIGGER TR_Workouts_BeforeUserRegistration ON [dbo].[Workouts]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.Id FROM inserted i
                JOIN dbo.Users u ON u.Id = i.UserId
                WHERE i.[StartedAt] < u.[CreatedAt]
            )
            BEGIN
                RAISERROR ('Workout start date can''t be before user registration date', 16, 1);
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END");

    public static OperationBuilder<SqlOperation> DropWorkoutDateTrigger(this MigrationBuilder migrationBuilder)
        => migrationBuilder.Sql(@"
        IF OBJECT_ID('TR_Workouts_BeforeUserRegistration', 'TR') IS NOT NULL
        DROP TRIGGER TR_Workouts_BeforeUserRegistration;");
}
