namespace PrintFarm_Manager.ViewModels.PrintJob;

public class PrintJobIndexViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Filaments { get; set; } = string.Empty;
    public decimal EstimatedHours { get; set; }
    public string PrinterName { get; set; } = string.Empty;
}
