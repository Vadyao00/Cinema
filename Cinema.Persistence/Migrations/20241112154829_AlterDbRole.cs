using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterDbRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04f1f63e-7519-4053-8346-95edabf2cf1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "356a97d6-7f10-4752-a63f-e58dea426b89");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a474b86-bd85-4c4f-9177-3cfeb709ab1e", null, "Administrator", "ADMINISTRATOR" },
                    { "1e1f08bb-628f-4037-991a-0f44d8d0f7bf", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a474b86-bd85-4c4f-9177-3cfeb709ab1e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e1f08bb-628f-4037-991a-0f44d8d0f7bf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04f1f63e-7519-4053-8346-95edabf2cf1f", null, "User", "User" },
                    { "356a97d6-7f10-4752-a63f-e58dea426b89", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
