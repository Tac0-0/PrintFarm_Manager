namespace PrintFarm_Manager.ViewModels.Filament;

public class FilamentDeleteViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PrintJobsCount { get; set; }
}
