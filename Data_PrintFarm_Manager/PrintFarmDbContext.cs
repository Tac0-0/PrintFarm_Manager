using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_PrintFarm_Manager;

public class PrintFarmDbContext : DbContext
{
    public PrintFarmDbContext(DbContextOptions<PrintFarmDbContext> options)
        : base(options)
    {
    }

    public DbSet<Printer> Printers { get; set; } = null!;
    public DbSet<PrintJob> PrintJobs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PrintJob>()
            .HasOne(j => j.Printer)
            .WithMany(p => p.PrintJobs)
            .HasForeignKey(j => j.PrinterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
