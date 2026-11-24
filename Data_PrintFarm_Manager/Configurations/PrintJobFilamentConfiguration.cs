using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_PrintFarm_Manager.Configurations;

public class PrintJobFilamentConfiguration : IEntityTypeConfiguration<PrintJobFilament>
{
    public void Configure(EntityTypeBuilder<PrintJobFilament> builder)
    {
        builder.HasKey(link => new { link.PrintJobId, link.FilamentId });

        builder.HasOne(link => link.PrintJob)
            .WithMany(j => j.PrintJobFilaments)
            .HasForeignKey(link => link.PrintJobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(link => link.Filament)
            .WithMany(f => f.PrintJobFilaments)
            .HasForeignKey(link => link.FilamentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
