using System.ComponentModel.DataAnnotations;

namespace Data_PrintFarm_Manager.Entities;

public class Printer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string Model { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Location { get; set; } = string.Empty;

    public bool IsOperational { get; set; } = true;

    public ICollection<PrintJob> PrintJobs { get; set; } = new List<PrintJob>();
}
