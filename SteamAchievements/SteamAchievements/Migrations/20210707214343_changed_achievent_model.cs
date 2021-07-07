using Microsoft.EntityFrameworkCore.Migrations;

namespace SteamAchievements.Migrations
{
    public partial class changed_achievent_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e2da0e2-d60e-4c4f-bc1b-2facb384fcb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd4cd864-9841-4ec9-a625-3dbba93ccb8f");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e7532d0-56e6-494f-a4ea-af37da94b35e", "7d7e5b74-5627-47f0-91b7-f8dd1b5b47be", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "23dff3ba-7524-4c60-a163-d39f6d514790", "c567fb09-2404-4884-be06-112f2a181c3d", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23dff3ba-7524-4c60-a163-d39f6d514790");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e7532d0-56e6-494f-a4ea-af37da94b35e");

            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e2da0e2-d60e-4c4f-bc1b-2facb384fcb5", "429d2f87-300d-4808-86a2-2145f86f084d", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fd4cd864-9841-4ec9-a625-3dbba93ccb8f", "ea3baecb-7a18-4381-9cdb-0b98669880d6", "Administrator", "ADMINISTRATOR" });
        }
    }
}
