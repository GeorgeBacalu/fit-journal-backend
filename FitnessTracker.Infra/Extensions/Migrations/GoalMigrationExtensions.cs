using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

public static class GoalMigrationExtensions
{
    public static OperationBuilder<SqlOperation> AddGoalStartDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        CREATE TRIGGER TR_Goals_BeforeUserRegistration ON [Goals]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN [Users] u ON u.[Id] = i.[UserId]
                WHERE i.[StartDate] < u.[CreatedAt]
            )
            BEGIN
                THROW 50002, 'Goal start date can''t be before user registration date', 1;
            END
        END;");

    public static OperationBuilder<SqlOperation> AddGoalValidateWeightTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        CREATE TRIGGER TR_Goals_ValitateWeight ON [Goals]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                WHERE i.[Type] = 'WeightLoss' AND i.[TargetWeight] >= (
                    SELECT u.[Weight] FROM [Users] u
                    JOIN inserted ins ON u.[Id] = ins.[UserId]
                )
            )
            BEGIN
                THROW 50003, 'Target weight for Weight Loss goals must be less than current user weight', 1;
            END
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                WHERE i.[Type] = 'WeightGain' AND i.[TargetWeight] <= (
                    SELECT u.[Weight] FROM [Users] u
                    JOIN inserted ins ON u.[Id] = ins.[UserId]
                )
            )
            BEGIN
                THROW 50004, 'Target weight for Weight Gain goals must be greater than current user weight', 1;
            END
        END;");

    public static OperationBuilder<SqlOperation> AddGoalOverlappingTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        CREATE TRIGGER TR_Goals_ValidateOverlapping ON [Goals]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN [Goals] g ON i.[UserId] = g.[UserId]
                WHERE i.[Id] <> g.[Id]
                  AND i.[Type] = g.[Type]
                  AND i.[StartDate] <= g.[EndDate]
                  AND i.[EndDate] >= g.[StartDate]
            )
            
            BEGIN
                THROW 50005, 'Users can''t have overlapping goals', 1;
            END
        END;");

    public static OperationBuilder<SqlOperation> DropGoalStartDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        IF OBJECT_ID('TR_Goals_BeforeUserRegistration', 'TR') IS NOT NULL
        DROP TRIGGER TR_Goals_BeforeUserRegistration;");

    public static OperationBuilder<SqlOperation> DropGoalValidateWeightTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        IF OBJECT_ID('TR_Goals_ValitateWeight', 'TR') IS NOT NULL
        DROP TRIGGER TR_Goals_ValitateWeight;");

    public static OperationBuilder<SqlOperation> DropGoalOverlappingTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        IF OBJECT_ID('TR_Goals_ValidateOverlapping', 'TR') IS NOT NULL
        DROP TRIGGER TR_Goals_ValidateOverlapping;");
}
