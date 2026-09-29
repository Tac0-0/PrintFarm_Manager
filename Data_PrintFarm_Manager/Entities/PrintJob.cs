using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_PrintFarm_Manager.Entities;

public class PrintJob
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Material { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.1", "500", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    [Column(TypeName = "decimal(6,2)")]
    public decimal EstimatedHours { get; set; }

    public DateTime CreatedOn { get; set; }

    [ForeignKey(nameof(Printer))]
    public int PrinterId { get; set; }

    public Printer Printer { get; set; } = null!;
}
