using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_PrintFarm_Manager.Configurations;

public class PrinterConfiguration : IEntityTypeConfiguration<Printer>
{
    public void Configure(EntityTypeBuilder<Printer> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Model).IsRequired().HasMaxLength(60);
        builder.Property(p => p.Location).IsRequired().HasMaxLength(80);

        builder.HasIndex(p => p.Name);
    }
}
