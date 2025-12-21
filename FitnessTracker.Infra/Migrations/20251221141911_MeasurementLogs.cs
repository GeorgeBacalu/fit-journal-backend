using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MeasurementLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workout_DurationMinutes",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workout_StartedAt",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkoutExercise_Reps",
                table: "WorkoutExercises");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkoutExercise_Sets",
                table: "WorkoutExercises");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Height",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodLog_Date",
                table: "FoodLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodLog_Quantity",
                table: "FoodLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodLog_Servings",
                table: "FoodLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItem_Calories",
                table: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItem_Carbs",
                table: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItem_Fat",
                table: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItem_Protein",
                table: "FoodItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkoutId",
                table: "WorkoutExercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExerciseId",
                table: "WorkoutExercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Users",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "Users",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "MeasurementLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    BodyFatPercentage = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    WaistCircumference = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    ChestCircumference = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    ArmsCircumference = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementLogs", x => x.Id);
                    table.CheckConstraint("CK_MeasurementLogs_ArmsCircumference", "[ArmsCircumference] BETWEEN 10 AND 100");
                    table.CheckConstraint("CK_MeasurementLogs_BodyFatPercentage", "[BodyFatPercentage] BETWEEN 2 AND 60");
                    table.CheckConstraint("CK_MeasurementLogs_ChestCircumference", "[ChestCircumference] BETWEEN 30 AND 200");
                    table.CheckConstraint("CK_MeasurementLogs_Date", "[Date] <= CURRENT_TIMESTAMP");
                    table.CheckConstraint("CK_MeasurementLogs_WaistCircumference", "[WaistCircumference] BETWEEN 30 AND 250");
                    table.CheckConstraint("CK_MeasurementLogs_Weight", "[Weight] BETWEEN 25 AND 250");
                    table.ForeignKey(
                        name: "FK_MeasurementLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workouts_DurationMinutes",
                table: "Workouts",
                sql: "[DurationMinutes] BETWEEN 5 AND 300");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workouts_StartedAt",
                table: "Workouts",
                sql: "[StartedAt] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkoutExercises_Reps",
                table: "WorkoutExercises",
                sql: "[Reps] BETWEEN 1 AND 50");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkoutExercises_Sets",
                table: "WorkoutExercises",
                sql: "[Sets] BETWEEN 1 AND 10");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkoutExercises_WeightUsed",
                table: "WorkoutExercises",
                sql: "[WeightUsed] BETWEEN 1 AND 500");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodLogs_Date",
                table: "FoodLogs",
                sql: "[Date] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodLogs_Quantity",
                table: "FoodLogs",
                sql: "[Quantity] BETWEEN 100 AND 5000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodLogs_Servings",
                table: "FoodLogs",
                sql: "[Servings] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItems_Calories",
                table: "FoodItems",
                sql: "[Calories] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItems_Carbs",
                table: "FoodItems",
                sql: "[Carbs] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItems_Fat",
                table: "FoodItems",
                sql: "[Fat] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItems_Protein",
                table: "FoodItems",
                sql: "[Protein] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementLogs_UserId_Date",
                table: "MeasurementLogs",
                columns: new[] { "UserId", "Date" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts");

            migrationBuilder.DropTable(
                name: "MeasurementLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workouts_DurationMinutes",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Workouts_StartedAt",
                table: "Workouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkoutExercises_Reps",
                table: "WorkoutExercises");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkoutExercises_Sets",
                table: "WorkoutExercises");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkoutExercises_WeightUsed",
                table: "WorkoutExercises");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodLogs_Date",
                table: "FoodLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodLogs_Quantity",
                table: "FoodLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodLogs_Servings",
                table: "FoodLogs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItems_Calories",
                table: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItems_Carbs",
                table: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItems_Fat",
                table: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FoodItems_Protein",
                table: "FoodItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkoutId",
                table: "WorkoutExercises",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExerciseId",
                table: "WorkoutExercises",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Users",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "Users",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workout_DurationMinutes",
                table: "Workouts",
                sql: "[DurationMinutes] BETWEEN 5 AND 300");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Workout_StartedAt",
                table: "Workouts",
                sql: "[StartedAt] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkoutExercise_Reps",
                table: "WorkoutExercises",
                sql: "[Reps] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkoutExercise_Sets",
                table: "WorkoutExercises",
                sql: "[Sets] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Height",
                table: "Users",
                sql: "[Height] BETWEEN 120 AND 250");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users",
                sql: "[Weight] BETWEEN 25 AND 250");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodLog_Date",
                table: "FoodLogs",
                sql: "[Date] <= CURRENT_TIMESTAMP");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodLog_Quantity",
                table: "FoodLogs",
                sql: "[Quantity] BETWEEN 100 AND 5000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodLog_Servings",
                table: "FoodLogs",
                sql: "[Servings] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItem_Calories",
                table: "FoodItems",
                sql: "[Calories] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItem_Carbs",
                table: "FoodItems",
                sql: "[Carbs] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItem_Fat",
                table: "FoodItems",
                sql: "[Fat] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FoodItem_Protein",
                table: "FoodItems",
                sql: "[Protein] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Users_UserId",
                table: "Workouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
