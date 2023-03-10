using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Orange.Data.Sqlite.Migrations
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
                name: "ConfigurationEvents",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    EnvironmentName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    ElapsedTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ClientId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    HostName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderType = table.Column<string>(type: "TEXT", unicode: false, maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Tag = table.Column<string>(type: "TEXT", unicode: false, maxLength: 12, nullable: false),
                    ProcessorType = table.Column<string>(type: "TEXT", unicode: false, maxLength: 2048, nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingFileCounts",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingFileCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingFiles",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    EnvironmentName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "ProviderProperties",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    Value = table.Column<string>(type: "TEXT", unicode: false, nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderProperties_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "Orange",
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationEvents",
                schema: "Orange",
                table: "ConfigurationEvents",
                columns: new[] { "ApplicationName", "EnvironmentName", "ClientId", "HostName", "ElapsedTime", "CreatedOnUtc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProperties",
                schema: "Orange",
                table: "ProviderProperties",
                columns: new[] { "ProviderId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provider_Tags",
                schema: "Orange",
                table: "Providers",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Providers",
                schema: "Orange",
                table: "Providers",
                columns: new[] { "Name", "IsDisabled", "ProviderType", "ProcessorType" });

            migrationBuilder.CreateIndex(
                name: "IX_SettingFileCounts",
                schema: "Orange",
                table: "SettingFileCounts",
                column: "CreatedOnUtc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingFiles1",
                schema: "Orange",
                table: "SettingFiles",
                columns: new[] { "ApplicationName", "EnvironmentName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SettingsFiles",
                schema: "Orange",
                table: "SettingFiles",
                column: "IsDisabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationEvents",
                schema: "Orange");

            migrationBuilder.DropTable(
                name: "ProviderProperties",
                schema: "Orange");

            migrationBuilder.DropTable(
                name: "SettingFileCounts",
                schema: "Orange");

            migrationBuilder.DropTable(
                name: "SettingFiles",
                schema: "Orange");

            migrationBuilder.DropTable(
                name: "Providers",
                schema: "Orange");
        }
    }
}
