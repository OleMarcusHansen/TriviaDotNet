using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIOF.Net.V2023.DatabaseService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Correct = table.Column<int>(type: "int", nullable: false),
                    Wrong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HighScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Correct = table.Column<int>(type: "int", nullable: false),
                    Wrong = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighScores", x => new { x.Id, x.Category });
                    table.ForeignKey(
                        name: "FK_HighScores_UserData_Id",
                        column: x => x.Id,
                        principalTable: "UserData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HighScores");

            migrationBuilder.DropTable(
                name: "UserData");
        }
    }
}
