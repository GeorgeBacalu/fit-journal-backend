using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

internal static class MeasurementLogMigrationExtensions
{
    public static OperationBuilder<SqlOperation> AddMeasurementLogDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        CREATE TRIGGER TR_MeasurementLogs_BeforeUserRegistration ON [MeasurementLogs]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN [Users] u ON u.[Id] = i.[UserId]
                WHERE i.[Date] < u.[CreatedAt]
            )
            BEGIN
                THROW 50007, 'Measurement log date can''t be before user registration date', 1;
            END
        END;");

    public static OperationBuilder<SqlOperation> DropMeasurementLogDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        IF OBJECT_ID('TR_MeasurementLogs_BeforeUserRegistration', 'TR') IS NOT NULL
        DROP TRIGGER TR_MeasurementLogs_BeforeUserRegistration;");
}
