using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademiaPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seeding_Data_Admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "FullName", "Password", "Role" },
                values: new object[] { 1, "admin@university.com", "Admin User", "pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
