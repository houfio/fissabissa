using Microsoft.EntityFrameworkCore.Migrations;

namespace FissaBissa.Data.Migrations
{
    public partial class TypeSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AnimalTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Jungle" },
                    { 2, "Boerderij" },
                    { 3, "Sneeuw" },
                    { 4, "Woestijn" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnimalTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AnimalTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AnimalTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AnimalTypes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
