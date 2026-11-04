using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_PrintFarm_Manager.Migrations;

public partial class AddFilamentsAndJobLinks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Filaments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                Color = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Filaments", x => x.Id));

        migrationBuilder.CreateTable(
            name: "PrintJobFilaments",
            columns: table => new
            {
                PrintJobId = table.Column<int>(type: "int", nullable: false),
                FilamentId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PrintJobFilaments", x => new { x.PrintJobId, x.FilamentId });
                table.ForeignKey(
                    name: "FK_PrintJobFilaments_PrintJobs_PrintJobId",
                    column: x => x.PrintJobId,
                    principalTable: "PrintJobs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PrintJobFilaments_Filaments_FilamentId",
                    column: x => x.FilamentId,
                    principalTable: "Filaments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Filaments_Name_Color",
            table: "Filaments",
            columns: new[] { "Name", "Color" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PrintJobFilaments_FilamentId",
            table: "PrintJobFilaments",
            column: "FilamentId");

        // Preserve old material names and the jobs that use them before dropping the legacy column.
        migrationBuilder.Sql(@"
INSERT INTO [Filaments] ([Name], [Color])
SELECT DISTINCT LTRIM(RTRIM([Material])), N''
FROM [PrintJobs]
WHERE NULLIF(LTRIM(RTRIM([Material])), '') IS NOT NULL;

INSERT INTO [PrintJobFilaments] ([PrintJobId], [FilamentId])
SELECT j.[Id], f.[Id]
FROM [PrintJobs] AS j
INNER JOIN [Filaments] AS f ON f.[Name] = LTRIM(RTRIM(j.[Material]));
");

        migrationBuilder.DropColumn(name: "Material", table: "PrintJobs");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Material",
            table: "PrintJobs",
            type: "nvarchar(30)",
            maxLength: 30,
            nullable: false,
            defaultValue: "");

        // The old format supports one material per job; use the first association on rollback.
        migrationBuilder.Sql(@"
UPDATE j
SET [Material] = COALESCE((
    SELECT TOP (1) f.[Name]
    FROM [PrintJobFilaments] AS jf
    INNER JOIN [Filaments] AS f ON f.[Id] = jf.[FilamentId]
    WHERE jf.[PrintJobId] = j.[Id]
    ORDER BY f.[Id]
), '')
FROM [PrintJobs] AS j;
");

        migrationBuilder.DropTable(name: "PrintJobFilaments");
        migrationBuilder.DropTable(name: "Filaments");
    }
}
