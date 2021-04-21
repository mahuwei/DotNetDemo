using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicallyDbConnectionString.Migrations
{
    public partial class EmployeeAddMobileNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "Employees");
        }
    }
}
