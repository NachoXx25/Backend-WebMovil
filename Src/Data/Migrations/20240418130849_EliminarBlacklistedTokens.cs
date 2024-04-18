using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taller1WebMovil.Src.Data.Migrations
{
    public partial class EliminarBlacklistedTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}