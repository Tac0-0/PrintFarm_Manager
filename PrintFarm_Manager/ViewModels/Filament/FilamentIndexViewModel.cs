namespace PrintFarm_Manager.ViewModels.Filament;

public class FilamentIndexViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public int PrintJobsCount { get; set; }
}
