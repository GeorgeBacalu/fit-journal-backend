using FitJournal.Infra.Constants;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitJournal.Infra.Extensions.Migrations;

internal static class FoodLogMigrationExtensions
{
    internal static OperationBuilder<SqlOperation> AddFoodLogBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        CREATE TRIGGER {DbTriggers.FoodLogsBeforeRegistration} ON [dbo].[FoodLogs]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN [dbo].[Users] u ON u.[Id] = i.[UserId]
                WHERE i.[DeletedAt] IS NULL AND u.[DeletedAt] IS NULL AND i.[Date] < u.[CreatedAt]
            ) THROW 50005, {DbErrors.FoodLogs.TriggerBeforeRegistration}, 1;
        END;");

    internal static OperationBuilder<SqlOperation> DropFoodLogBeforeRegistrationTrigger(this MigrationBuilder builder) =>
        builder.Sql($@"
        IF OBJECT_ID('{DbTriggers.FoodLogsBeforeRegistration}', 'TR') IS NOT NULL
        DROP TRIGGER {DbTriggers.FoodLogsBeforeRegistration};");
}
