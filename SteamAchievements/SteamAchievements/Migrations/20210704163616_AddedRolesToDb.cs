using Microsoft.EntityFrameworkCore.Migrations;

namespace SteamAchievements.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e2da0e2-d60e-4c4f-bc1b-2facb384fcb5", "429d2f87-300d-4808-86a2-2145f86f084d", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fd4cd864-9841-4ec9-a625-3dbba93ccb8f", "ea3baecb-7a18-4381-9cdb-0b98669880d6", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e2da0e2-d60e-4c4f-bc1b-2facb384fcb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd4cd864-9841-4ec9-a625-3dbba93ccb8f");
        }
    }
}
