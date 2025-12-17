using FitnessTracker.Infra.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class GoalTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddGoalStartDateTrigger();
            migrationBuilder.AddGoalValidateWeightTrigger();
            migrationBuilder.AddGoalOverlappingTrigger();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropGoalStartDateTrigger();
            migrationBuilder.DropGoalValidateWeightTrigger();
            migrationBuilder.DropGoalOverlappingTrigger();
        }
    }
}
