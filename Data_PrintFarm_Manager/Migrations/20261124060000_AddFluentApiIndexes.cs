using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_PrintFarm_Manager.Migrations;

public partial class AddFluentApiIndexes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Printers_Name",
            table: "Printers",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_PrintJobs_CreatedOn",
            table: "PrintJobs",
            column: "CreatedOn");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Printers_Name",
            table: "Printers");

        migrationBuilder.DropIndex(
            name: "IX_PrintJobs_CreatedOn",
            table: "PrintJobs");
    }
}
