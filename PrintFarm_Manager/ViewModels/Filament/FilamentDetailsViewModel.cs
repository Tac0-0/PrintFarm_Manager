namespace PrintFarm_Manager.ViewModels.Filament;

public class FilamentDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public List<string> PrintJobs { get; set; } = new();
}
