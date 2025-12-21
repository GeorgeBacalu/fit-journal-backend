using FitnessTracker.Infra.Extensions.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class WorkoutStartDateTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) =>
            migrationBuilder.AddWorkoutStartDateTrigger();

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.DropWorkoutStartDateTrigger();
    }
}
