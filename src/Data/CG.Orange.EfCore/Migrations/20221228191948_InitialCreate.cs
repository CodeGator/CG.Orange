using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Orange.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Orange");

            migrationBuilder.CreateTable(
                name: "SettingFiles",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    EnvironmentName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileSettings",
                schema: "Orange",
                table: "SettingFiles",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_FileSettings_Application",
                schema: "Orange",
                table: "SettingFiles",
                columns: new[] { "ApplicationName", "EnvironmentName" },
                unique: true,
                filter: "[EnvironmentName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingFiles",
                schema: "Orange");
        }
    }
}
