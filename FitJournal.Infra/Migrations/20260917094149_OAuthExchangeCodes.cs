using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitJournal.Infra.Migrations
{
    /// <inheritdoc />
    public partial class OAuthExchangeCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OAuthExchangeCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeHash = table.Column<string>(type: "nchar(64)", fixedLength: true, maxLength: 64, nullable: false),
                    TokenPayload = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConsumedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthExchangeCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OAuthExchangeCodes_CodeHash",
                table: "OAuthExchangeCodes",
                column: "CodeHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OAuthExchangeCodes_ExpiresAt",
                table: "OAuthExchangeCodes",
                column: "ExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OAuthExchangeCodes");
        }
    }
}
