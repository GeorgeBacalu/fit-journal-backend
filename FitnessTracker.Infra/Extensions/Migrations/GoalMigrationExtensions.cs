using FitnessTracker.Infra.Constants;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

internal static class GoalMigrationExtensions
{
    internal static OperationBuilder<SqlOperation> AddGoalBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.GoalsBeforeRegistration} ON [dbo].[Goals]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;

            IF EXISTS (
                SELECT 1 FROM inserted i
                JOIN [dbo].[Users] u ON u.[Id] = i.[UserId]
                WHERE i.[DeletedAt] IS NULL AND u.[DeletedAt] IS NULL AND i.[StartDate] < u.[CreatedAt]
            ) THROW 50002, {DbErrors.Goals.TriggerBeforeRegistration}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> AddGoalWeightValidationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.GoalsValidateWeight} ON [dbo].[Goals]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;

            IF EXISTS (
                SELECT 1 FROM inserted i
                JOIN [dbo].[Users] u ON u.[Id] = i.[UserId]
                WHERE i.[DeletedAt] IS NULL AND u.[DeletedAt] IS NULL
                AND ((i.[Type] = 'WeightLoss' AND i.[TargetWeight] >= u.[Weight])
                  OR (i.[Type] = 'WeightGain' AND i.[TargetWeight] <= u.[Weight]))
            ) THROW 50003, {DbErrors.Goals.TriggerValidateWeight}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> AddGoalOverlapValidationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.GoalsValidateOverlapping} ON [dbo].[Goals]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;

            IF EXISTS (
                SELECT 1 FROM inserted i
                JOIN [dbo].[Goals] g ON i.[UserId] = g.[UserId] AND i.[Id] <> g.[Id]
                WHERE i.[DeletedAt] IS NULL AND g.[DeletedAt] IS NULL 
                  AND i.[Type] = g.[Type] AND i.[StartDate] <= g.[EndDate] AND i.[EndDate] >= g.[StartDate]
            ) THROW 50004, {DbErrors.Goals.TriggerValidateOverlapping}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> DropGoalBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.GoalsBeforeRegistration}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.GoalsBeforeRegistration};");

    internal static OperationBuilder<SqlOperation> DropGoalWeightValidationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.GoalsValidateWeight}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.GoalsValidateWeight};");

    internal static OperationBuilder<SqlOperation> DropGoalOverlapValidationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.GoalsValidateOverlapping}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.GoalsValidateOverlapping};");
}
