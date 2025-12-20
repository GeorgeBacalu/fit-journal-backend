using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace FitnessTracker.Infra.Extensions.Migrations;

public static class FoodLogMigrationExtensions
{
    public static OperationBuilder<SqlOperation> AddFoodLogDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        CREATE TRIGGER TR_FoodLog_BeforeUserRegistration ON [FoodLogs]
        AFTER INSERT, UPDATE
        AS BEGIN
            SET NOCOUNT ON;
            
            IF EXISTS (
                SELECT i.[Id] FROM inserted i
                JOIN [Users] u ON u.[Id] = i.[UserId]
                WHERE i.[Date] < u.[CreatedAt]
            )
            BEGIN
                THROW 50006, 'Food log date can''t be before user registration date', 1;
            END
        END;");

    public static OperationBuilder<SqlOperation> DropFoodLogDateTrigger(this MigrationBuilder builder) =>
        builder.Sql(@"
        IF OBJECT_ID('TR_FoodLog_BeforeUserRegistration', 'TR') IS NOT NULL
        DROP TRIGGER TR_FoodLog_BeforeUserRegistration;");
}
