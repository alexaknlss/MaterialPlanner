using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeysToMaterialDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MaterialDetails_BrandId",
                table: "MaterialDetails",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDetails_MaterialId",
                table: "MaterialDetails",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDetails_PresentationId",
                table: "MaterialDetails",
                column: "PresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDetails_ProductId",
                table: "MaterialDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialDetails_Brands_BrandId",
                table: "MaterialDetails",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialDetails_Materials_MaterialId",
                table: "MaterialDetails",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialDetails_Presentations_PresentationId",
                table: "MaterialDetails",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialDetails_Products_ProductId",
                table: "MaterialDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialDetails_Brands_BrandId",
                table: "MaterialDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialDetails_Materials_MaterialId",
                table: "MaterialDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialDetails_Presentations_PresentationId",
                table: "MaterialDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialDetails_Products_ProductId",
                table: "MaterialDetails");

            migrationBuilder.DropIndex(
                name: "IX_MaterialDetails_BrandId",
                table: "MaterialDetails");

            migrationBuilder.DropIndex(
                name: "IX_MaterialDetails_MaterialId",
                table: "MaterialDetails");

            migrationBuilder.DropIndex(
                name: "IX_MaterialDetails_PresentationId",
                table: "MaterialDetails");

            migrationBuilder.DropIndex(
                name: "IX_MaterialDetails_ProductId",
                table: "MaterialDetails");
        }
    }
}
