using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialPlanner.Migrations
{
    /// <inheritdoc />
    public partial class CreateMaterialDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MaterialDetails",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MaterialDetails",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MaterialDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MaterialDetails");
        }
    }
}
