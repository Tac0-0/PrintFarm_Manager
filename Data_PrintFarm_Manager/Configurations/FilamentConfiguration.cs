using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_PrintFarm_Manager.Configurations;

public class FilamentConfiguration : IEntityTypeConfiguration<Filament>
{
    public void Configure(EntityTypeBuilder<Filament> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name).IsRequired().HasMaxLength(30);
        builder.Property(f => f.Color).IsRequired().HasMaxLength(40);
        builder.Ignore(f => f.DisplayName);

        builder.HasIndex(f => new { f.Name, f.Color })
            .IsUnique();
    }
}
