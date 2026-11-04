# PrintFarm Manager

An ASP.NET Core MVC + Entity Framework Core 8 application for managing 3D printers, print jobs and filament types.

## Project architecture

- `Data_PrintFarm_Manager`: EF Core entities, SQL Server `PrintFarmDbContext`, and migrations.
- `PrintFarm_Manager_Core`: service interfaces and implementations; database queries and mutations live here.
- `PrintFarm_Manager`: MVC controllers, view models, Razor views, dependency injection and UI.

The MVC controllers depend on interfaces injected through `Program.cs`, not on the EF Core context.

## Database (four tables)

1. `Printers`: printer properties and operational status.
2. `PrintJobs`: job details and a `PrinterId` foreign key (one printer has many jobs).
3. `Filaments`: reusable material entries (for example PLA or PETG), optionally distinguished by color. A name/color pair is unique.
4. `PrintJobFilaments`: **connecting table** with composite primary key `(PrintJobId, FilamentId)`. It models the many-to-many relation between jobs and materials.

The web UI provides CRUD for printers, print jobs, and filaments. A print job can select multiple materials. Deleting a filament removes its junction rows, not the print jobs.

## Setup

1. Install .NET 8 SDK and SQL Server.
2. Set `ConnectionStrings:DefaultConnection` in `PrintFarm_Manager/appsettings.json` or via user secrets to point at your SQL Server. The checked-in `Server=TEACHER` value is specific to the original classroom setup.
3. From the repository root, apply migrations:

   ```bash
   dotnet restore PrintFarm_Manager.sln
   dotnet ef database update --project Data_PrintFarm_Manager --startup-project PrintFarm_Manager
   dotnet run --project PrintFarm_Manager
   ```

The `AddFilamentsAndJobLinks` migration creates the new tables and **migrates existing `PrintJobs.Material` values** to reusable `Filaments` rows and junction records before dropping the old column. A rollback can only retain one material per job because the original schema supported only one.

Create a printer and at least one filament before adding a print job.
