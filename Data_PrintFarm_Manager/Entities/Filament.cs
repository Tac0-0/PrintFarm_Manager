using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_PrintFarm_Manager.Entities;

public class Filament
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; } = string.Empty;

    [StringLength(40)]
    public string Color { get; set; } = string.Empty;

    [NotMapped]
    public string DisplayName => string.IsNullOrWhiteSpace(Color) ? Name : $"{Name} ({Color})";

    public ICollection<PrintJobFilament> PrintJobFilaments { get; set; } = new List<PrintJobFilament>();
}
