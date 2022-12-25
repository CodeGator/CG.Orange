using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Orange.SqlLite.Migrations
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationName = table.Column<string>(type: "TEXT", nullable: false),
                    EnvironmentName = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalFileName = table.Column<string>(type: "TEXT", unicode: false, maxLength: 260, nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Json = table.Column<string>(type: "TEXT", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                unique: true);
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
