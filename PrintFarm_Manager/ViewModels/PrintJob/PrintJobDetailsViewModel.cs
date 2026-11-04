namespace PrintFarm_Manager.ViewModels.PrintJob;

public class PrintJobDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Filaments { get; set; } = string.Empty;
    public decimal EstimatedHours { get; set; }
    public DateTime CreatedOn { get; set; }
    public string PrinterName { get; set; } = string.Empty;
    public string PrinterLocation { get; set; } = string.Empty;
}
