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

        modelBuilder.Entity<PrintJob>()
            .HasOne(j => j.Printer)
            .WithMany(p => p.PrintJobs)
            .HasForeignKey(j => j.PrinterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Filament>()
            .HasIndex(f => new { f.Name, f.Color })
            .IsUnique();

        modelBuilder.Entity<PrintJobFilament>()
            .HasKey(link => new { link.PrintJobId, link.FilamentId });

        modelBuilder.Entity<PrintJobFilament>()
            .HasOne(link => link.PrintJob)
            .WithMany(j => j.PrintJobFilaments)
            .HasForeignKey(link => link.PrintJobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PrintJobFilament>()
            .HasOne(link => link.Filament)
            .WithMany(f => f.PrintJobFilaments)
            .HasForeignKey(link => link.FilamentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
