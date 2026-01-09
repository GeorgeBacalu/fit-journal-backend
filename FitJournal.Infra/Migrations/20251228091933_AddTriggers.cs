using FitJournal.Infra.Extensions.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitJournal.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddWorkoutBeforeRegistrationTrigger();

            migrationBuilder.AddGoalBeforeRegistrationTrigger();
            migrationBuilder.AddGoalWeightValidationTrigger();
            migrationBuilder.AddGoalOverlapValidationTrigger();

            migrationBuilder.AddFoodLogBeforeRegistrationTrigger();

            migrationBuilder.AddProgressLogBeforeRegistrationTrigger();
            migrationBuilder.AddProgressLogWeightChangeLimitTrigger();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropWorkoutBeforeRegistrationTrigger();

            migrationBuilder.DropGoalBeforeRegistrationTrigger();
            migrationBuilder.DropGoalWeightValidationTrigger();
            migrationBuilder.DropGoalOverlapValidationTrigger();

            migrationBuilder.DropFoodLogBeforeRegistrationTrigger();

            migrationBuilder.DropProgressLogBeforeRegistrationTrigger();
            migrationBuilder.DropProgressLogWeightChangeLimitTrigger();
        }
    }
}
