using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListings.Api.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85a85dec-f6b0-48f6-967d-081d375ed2c8", "67b086a3-9a3b-4d98-bf6e-6e99f8146af2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f96a1914-7f74-4fa3-8360-fbda15cff412", "f5a49c02-9436-4f72-92dc-dcd2e932d0d4", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85a85dec-f6b0-48f6-967d-081d375ed2c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f96a1914-7f74-4fa3-8360-fbda15cff412");
        }
    }
}
