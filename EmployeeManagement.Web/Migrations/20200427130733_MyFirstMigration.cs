using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmployeeManagement.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ntt_Employee_Records",
                columns: table => new
                {
                    NttEmployeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Password = table.Column<string>(maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ntt_Empl__EE5518D92A8BFB6D", x => x.NttEmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Ntt_Employee_Time_Details",
                columns: table => new
                {
                    EmployeeTImeDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NttEmployeeId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    InTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    OutTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ntt_Empl__CEF11F0DEA8B01DC", x => x.EmployeeTImeDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Ntt_Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: false),
                    Role = table.Column<string>(unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ntt_Role__8AFACE1A903E4BA5", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Ntt_Empl__7AD04F1062787924",
                table: "Ntt_Employee_Records",
                column: "EmployeeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ntt_Employee_Records");

            migrationBuilder.DropTable(
                name: "Ntt_Employee_Time_Details");

            migrationBuilder.DropTable(
                name: "Ntt_Roles");
        }
    }
}
