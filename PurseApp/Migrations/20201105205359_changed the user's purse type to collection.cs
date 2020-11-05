using Microsoft.EntityFrameworkCore.Migrations;

namespace PurseApp.Migrations
{
    public partial class changedtheuserspursetypetocollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Purses_UserId",
                table: "Purses");

            migrationBuilder.CreateIndex(
                name: "IX_Purses_UserId",
                table: "Purses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Purses_UserId",
                table: "Purses");

            migrationBuilder.CreateIndex(
                name: "IX_Purses_UserId",
                table: "Purses",
                column: "UserId",
                unique: true);
        }
    }
}
