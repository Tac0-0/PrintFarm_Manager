namespace PrintFarm_Manager.ViewModels.Printer;

public class PrinterDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsOperational { get; set; }
    public List<string> PrintJobs { get; set; } = new();
}
