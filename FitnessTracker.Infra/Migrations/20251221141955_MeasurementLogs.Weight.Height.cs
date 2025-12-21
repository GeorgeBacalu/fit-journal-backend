using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MeasurementLogsWeightHeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Height",
                table: "Users",
                sql: "[Height] BETWEEN 120 AND 250");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users",
                sql: "[Weight] BETWEEN 25 AND 250");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Height",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Weight",
                table: "Users");
        }
    }
}
