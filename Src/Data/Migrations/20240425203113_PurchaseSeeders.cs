using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taller1WebMovil.Src.Data.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseSeeders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProdcutType",
                table: "Purchases",
                newName: "ProductType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductType",
                table: "Purchases",
                newName: "ProdcutType");
        }
    }
}
