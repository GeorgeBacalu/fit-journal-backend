using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Workouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Birthday",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Email",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Height",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nchar(60)",
                fixedLength: true,
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(65)",
                oldMaxLength: 65);

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.CheckConstraint("CK_Workout_Date", "[StartedAt] <= CURRENT_TIMESTAMP");
                    table.CheckConstraint("CK_Workout_DurationMinuts", "[DurationMinutes] BETWEEN 5 AND 300");
                    table.ForeignKey(
                        name: "FK_Workouts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Birthday",
                table: "Users",
                sql: "[Birthday] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Email",
                table: "Users",
                sql: "[Email] LIKE '%_@__%.__%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Height",
                table: "Users",
                sql: "[Height] BETWEEN 120 AND 250");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users",
                sql: "[Weight] BETWEEN 25 AND 250");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_Name",
                table: "Workouts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "Workouts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Birthday",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Email",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Height",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(60)",
                oldFixedLength: true,
                oldMaxLength: 60);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Birthday",
                table: "Users",
                sql: "Birthday <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Email",
                table: "Users",
                sql: "Email LIKE '%_@__%.__%' AND Email NOT LIKE '% %'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Height",
                table: "Users",
                sql: "Height >= 0 AND Height <= 250");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users",
                sql: "Weight >= 0 AND Weight <= 250");
        }
    }
}
