using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_PrintFarm_Manager.Configurations;

public class PrintJobConfiguration : IEntityTypeConfiguration<PrintJob>
{
    public void Configure(EntityTypeBuilder<PrintJob> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title).IsRequired().HasMaxLength(100);
        builder.Property(j => j.EstimatedHours).HasColumnType("decimal(6,2)");

        builder.HasIndex(j => j.CreatedOn);

        builder.HasOne(j => j.Printer)
            .WithMany(p => p.PrintJobs)
            .HasForeignKey(j => j.PrinterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
