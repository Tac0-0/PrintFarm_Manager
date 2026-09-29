using System.ComponentModel.DataAnnotations;

namespace PrintFarm_Manager.ViewModels.Printer;

public class PrinterCreateViewModel
{
    [Required(ErrorMessage = "Името е задължително.")]
    [MaxLength(50)]
    [Display(Name = "Име")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моделът е задължителен.")]
    [MaxLength(60)]
    [Display(Name = "Модел")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Местоположението е задължително.")]
    [MaxLength(80)]
    [Display(Name = "Местоположение")]
    public string Location { get; set; } = string.Empty;

    [Display(Name = "Работи")]
    public bool IsOperational { get; set; } = true;
}
