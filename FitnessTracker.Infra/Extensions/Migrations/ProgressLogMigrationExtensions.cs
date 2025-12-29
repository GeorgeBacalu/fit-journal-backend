using FitnessTracker.Infra.Constants;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

internal static class ProgressLogMigrationExtensions
{
    internal static OperationBuilder<SqlOperation> AddProgressLogBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.ProgressLogsBeforeRegistration} ON [dbo].[ProgressLogs]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN [dbo].[Users] u ON u.[Id] = i.[UserId]
                WHERE i.[DeletedAt] IS NULL AND u.[DeletedAt] IS NULL AND i.[Date] < u.[CreatedAt]
            ) THROW 50006, {DbErrors.ProgressLogs.TriggerBeforeRegistration}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> AddProgressLogWeightChangeLimitTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.ProgressLogsWeightChangeLimit} ON [dbo].[ProgressLogs]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            DECLARE @MaxKgPerDay DECIMAL(3, 2) = 2.00;

            IF EXISTS (
                SELECT 1 FROM inserted i
                OUTER APPLY (
                    SELECT TOP (1) pl.[Date], pl.[Weight] FROM [dbo].[ProgressLogs] pl
                    WHERE pl.[UserId] = i.[UserId] AND pl.[DeletedAt] IS NULL AND pl.[Date] < i.[Date]
                    ORDER BY pl.[Date] DESC) l
                WHERE i.[DeletedAt] IS NULL AND l.[Date] IS NOT NULL AND ABS(i.[Weight] - l.[Weight]) > @MaxKgPerDay * DATEDIFF(DAY, l.[Date], i.[Date])
            ) THROW 50007, {DbErrors.ProgressLogs.TriggerWeightChangeLimit}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> DropProgressLogBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.ProgressLogsBeforeRegistration}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.ProgressLogsBeforeRegistration};");

    internal static OperationBuilder<SqlOperation> DropProgressLogWeightChangeLimitTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.ProgressLogsWeightChangeLimit}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.ProgressLogsWeightChangeLimit};");
}
