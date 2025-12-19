using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class FoodItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Goals_TargetWeight",
                table: "Goals");

            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Calories = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Carbs = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Fat = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                    table.CheckConstraint("CK_FoodItem_Calories", "[Calories] >= 0");
                    table.CheckConstraint("CK_FoodItem_Carbs", "[Carbs] >= 0");
                    table.CheckConstraint("CK_FoodItem_Fat", "[Fat] >= 0");
                    table.CheckConstraint("CK_FoodItem_Protein", "[Protein] >= 0");
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Goals_TargetWeight",
                table: "Goals",
                sql: "[TargetWeight] BETWEEN 25 AND 250");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_Name",
                table: "FoodItems",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Goals_TargetWeight",
                table: "Goals");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Goals_TargetWeight",
                table: "Goals",
                sql: "[TargetWeight] > 0");
        }
    }
}
