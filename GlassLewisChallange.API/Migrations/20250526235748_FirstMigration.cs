using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlassLewisChallange.API.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMPANIES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EXCHANGE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TICKER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ISIN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WEBSITE = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPANIES", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMPANIES_ISIN",
                table: "COMPANIES",
                column: "ISIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMPANIES");
        }
    }
}
