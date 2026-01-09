using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitJournal.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RequestLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnerExceptionType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    InnerExceptionMessage = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    InnerExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    QueryString = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RemoteIp = table.Column<long>(type: "bigint", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", maxLength: 10240, nullable: false),
                    RequestBodySize = table.Column<int>(type: "int", nullable: false),
                    RequestHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", maxLength: 10240, nullable: false),
                    ResponseBodySize = table.Column<int>(type: "int", nullable: false),
                    ResponseHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseStatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                    table.CheckConstraint("CK_RequestLogs_Duration", "[Duration] >= 0");
                    table.CheckConstraint("CK_RequestLogs_RequestBodySize", "[RequestBodySize] >= 0");
                    table.CheckConstraint("CK_RequestLogs_ResponseBodySize", "[ResponseBodySize] >= 0");
                    table.CheckConstraint("CK_RequestLogs_ResponseStatus", "[ResponseStatus] BETWEEN 100 AND 599");
                    table.ForeignKey(
                        name: "FK_RequestLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_CreatedAt",
                table: "RequestLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_Duration",
                table: "RequestLogs",
                column: "Duration");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_Method_Path",
                table: "RequestLogs",
                columns: new[] { "Method", "Path" });

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_ResponseStatus",
                table: "RequestLogs",
                column: "ResponseStatus");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_UserId",
                table: "RequestLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLogs");
        }
    }
}
