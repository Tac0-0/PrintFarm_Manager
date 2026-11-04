using System.ComponentModel.DataAnnotations;

namespace PrintFarm_Manager.ViewModels.Filament;

public class FilamentCreateViewModel
{
    [Required(ErrorMessage = "Името на материала е задължително.")]
    [MaxLength(30)]
    [Display(Name = "Материал")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(40)]
    [Display(Name = "Цвят")]
    public string? Color { get; set; }
}
