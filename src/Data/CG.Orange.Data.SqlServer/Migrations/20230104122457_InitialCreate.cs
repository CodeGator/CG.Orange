using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Orange.Data.SqlServer.Migrations
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
                name: "Providers",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderType = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Tag = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    ProcessorType = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ProviderProperties",
                schema: "Orange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false),
                    Value = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "IX_SettingFiles1",
                schema: "Orange",
                table: "SettingFiles",
                columns: new[] { "ApplicationName", "EnvironmentName" },
                unique: true,
                filter: "[EnvironmentName] IS NOT NULL");

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
                name: "ProviderProperties",
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
