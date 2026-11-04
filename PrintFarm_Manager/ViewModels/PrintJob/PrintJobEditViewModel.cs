using System.ComponentModel.DataAnnotations;

namespace PrintFarm_Manager.ViewModels.PrintJob;

public class PrintJobEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заглавието е задължително.")]
    [MaxLength(100)]
    [Display(Name = "Задание")]
    public string Title { get; set; } = string.Empty;

    [MinLength(1, ErrorMessage = "Изберете поне един материал.")]
    [Display(Name = "Материали")]
    public List<int> FilamentIds { get; set; } = new();

    [Range(typeof(decimal), "0.1", "500", ErrorMessage = "Времето трябва да е между 0.1 и 500 часа.", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    [Display(Name = "Очаквани часове")]
    public decimal EstimatedHours { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Изберете принтер.")]
    [Display(Name = "Принтер")]
    public int PrinterId { get; set; }
}
