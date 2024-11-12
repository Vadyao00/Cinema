using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterDbRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "964ce008-c88a-479c-aaad-0f7539aac2ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef4bfc91-6cff-4130-b9d1-98da015acc5d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04f1f63e-7519-4053-8346-95edabf2cf1f", null, "User", "User" },
                    { "356a97d6-7f10-4752-a63f-e58dea426b89", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "964ce008-c88a-479c-aaad-0f7539aac2ac", null, "User", "USER" },
                    { "ef4bfc91-6cff-4130-b9d1-98da015acc5d", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
