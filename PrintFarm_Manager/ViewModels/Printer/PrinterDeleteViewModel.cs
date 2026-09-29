namespace PrintFarm_Manager.ViewModels.Printer;

public class PrinterDeleteViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PrintJobsCount { get; set; }
}
