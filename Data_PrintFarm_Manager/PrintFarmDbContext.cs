using Data_PrintFarm_Manager.Configurations;
using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_PrintFarm_Manager;

public class PrintFarmDbContext : DbContext
{
    public PrintFarmDbContext(DbContextOptions<PrintFarmDbContext> options)
        : base(options) { }

    public DbSet<Printer> Printers { get; set; } = null!;
    public DbSet<PrintJob> PrintJobs { get; set; } = null!;
    public DbSet<Filament> Filaments { get; set; } = null!;
    public DbSet<PrintJobFilament> PrintJobFilaments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PrinterConfiguration());
        modelBuilder.ApplyConfiguration(new PrintJobConfiguration());
        modelBuilder.ApplyConfiguration(new FilamentConfiguration());
        modelBuilder.ApplyConfiguration(new PrintJobFilamentConfiguration());
    }
}
