using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreMVCAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaffPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "Id", "Name", "Position", "Salary" },
                values: new object[,]
                {
                    { 1, "John Doe", "Manager", 50000m },
                    { 2, "Jane Smith", "Developer", 40000m },
                    { 3, "Michael Brown", "Designer", 45000m },
                    { 4, "Emily Davis", "Tester", 35000m },
                    { 5, "Chris Johnson", "HR", 38000m },
                    { 6, "Sarah Wilson", "Accountant", 42000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
