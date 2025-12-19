using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class GoalsReponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workout_Date",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workout_DurationMinuts",
                table: "Workouts");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "WeightUsed",
                table: "WorkoutExercises",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWeight = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsAchieved = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.CheckConstraint("CK_Goals_Dates", "[StartDate] < [EndDate]");
                    table.CheckConstraint("CK_Goals_TargetWeight", "[TargetWeight] > 0");
                    table.ForeignKey(
                        name: "FK_Goals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workout_DurationMinutes",
                table: "Workouts",
                sql: "[DurationMinutes] BETWEEN 5 AND 300");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workout_StartedAt",
                table: "Workouts",
                sql: "[StartedAt] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_AgeRestriction",
                table: "Users",
                sql: "DATEDIFF(year, [Birthday], CURRENT_TIMESTAMP) >= 13");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workout_DurationMinutes",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workout_StartedAt",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_AgeRestriction",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WeightUsed",
                table: "WorkoutExercises",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workout_Date",
                table: "Workouts",
                sql: "[StartedAt] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workout_DurationMinuts",
                table: "Workouts",
                sql: "[DurationMinutes] BETWEEN 5 AND 300");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
