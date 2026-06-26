using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialPlanner.Migrations
{
    /// <inheritdoc />
    public partial class CreateUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "MaterialDetails");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "MaterialDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDetails_UnitId",
                table: "MaterialDetails",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialDetails_Units_UnitId",
                table: "MaterialDetails",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialDetails_Units_UnitId",
                table: "MaterialDetails");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_MaterialDetails_UnitId",
                table: "MaterialDetails");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "MaterialDetails");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "MaterialDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
