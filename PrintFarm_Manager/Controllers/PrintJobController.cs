using Data_PrintFarm_Manager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrintFarm_Manager.ViewModels.PrintJob;
using PrintFarm_Manager_Core.Interfaces;

namespace PrintFarm_Manager.Controllers;

public class PrintJobController : Controller
{
    private readonly IPrintJobService printJobService;
    private readonly IPrinterService printerService;
    private readonly IFilamentService filamentService;

    public PrintJobController(
        IPrintJobService printJobService, IPrinterService printerService, IFilamentService filamentService)
    {
        this.printJobService = printJobService;
        this.printerService = printerService;
        this.filamentService = filamentService;
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await printJobService.GetAllPrintJobsAsync();
        return View(jobs.Select(j => new PrintJobIndexViewModel
        {
            Id = j.Id, Title = j.Title, EstimatedHours = j.EstimatedHours,
            PrinterName = j.Printer.Name,
            Filaments = string.Join(", ", j.PrintJobFilaments
                .Select(link => link.Filament.DisplayName).OrderBy(name => name))
        }).ToList());
    }

    public async Task<IActionResult> Details(int id)
    {
        var job = await printJobService.GetPrintJobDetailsAsync(id);
        if (job == null) return NotFound();

        return View(new PrintJobDetailsViewModel
        {
            Id = job.Id, Title = job.Title, EstimatedHours = job.EstimatedHours,
            CreatedOn = job.CreatedOn, PrinterName = job.Printer.Name,
            PrinterLocation = job.Printer.Location,
            Filaments = string.Join(", ", job.PrintJobFilaments
                .Select(link => link.Filament.DisplayName).OrderBy(name => name))
        });
    }

    public async Task<IActionResult> Create()
    {
        await PopulateOptionsAsync();
        return View(new PrintJobCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrintJobCreateViewModel model)
    {
        await ValidateReferencesAsync(model.PrinterId, model.FilamentIds);
        if (!ModelState.IsValid)
        {
            await PopulateOptionsAsync(model.PrinterId, model.FilamentIds);
            return View(model);
        }

        await printJobService.AddPrintJobAsync(new PrintJob
        {
            Title = model.Title, EstimatedHours = model.EstimatedHours,
            CreatedOn = DateTime.Now, PrinterId = model.PrinterId
        }, model.FilamentIds);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var job = await printJobService.GetPrintJobByIdAsync(id);
        if (job == null) return NotFound();

        var model = new PrintJobEditViewModel
        {
            Id = job.Id, Title = job.Title, EstimatedHours = job.EstimatedHours,
            PrinterId = job.PrinterId,
            FilamentIds = job.PrintJobFilaments.Select(link => link.FilamentId).ToList()
        };
        await PopulateOptionsAsync(model.PrinterId, model.FilamentIds);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PrintJobEditViewModel model)
    {
        if (id != model.Id) return NotFound();

        await ValidateReferencesAsync(model.PrinterId, model.FilamentIds);
        if (!ModelState.IsValid)
        {
            await PopulateOptionsAsync(model.PrinterId, model.FilamentIds);
            return View(model);
        }

        var updated = await printJobService.UpdatePrintJobAsync(new PrintJob
        {
            Id = id, Title = model.Title,
            EstimatedHours = model.EstimatedHours, PrinterId = model.PrinterId
        }, model.FilamentIds);

        return updated ? RedirectToAction(nameof(Index)) : NotFound();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var job = await printJobService.GetPrintJobDetailsAsync(id);
        if (job == null) return NotFound();

        return View(new PrintJobDeleteViewModel
        {
            Id = job.Id, Title = job.Title, PrinterName = job.Printer.Name
        });
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await printJobService.DeletePrintJobAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateReferencesAsync(int printerId, List<int>? filamentIds)
    {
        var printers = await printerService.GetAllPrintersAsync();
        if (!printers.Any(p => p.Id == printerId))
            ModelState.AddModelError("PrinterId", "Избраният принтер не съществува.");

        var validFilamentIds = (await filamentService.GetAllFilamentsAsync()).Select(f => f.Id).ToHashSet();
        if (filamentIds == null || filamentIds.Count == 0)
            ModelState.AddModelError("FilamentIds", "Изберете поне един материал.");
        else if (filamentIds.Any(id => !validFilamentIds.Contains(id)))
            ModelState.AddModelError("FilamentIds", "Невалиден материал.");
    }

    private async Task PopulateOptionsAsync(int? selectedPrinterId = null, IEnumerable<int>? selectedFilamentIds = null)
    {
        ViewBag.Printers = new SelectList(
            await printerService.GetAllPrintersAsync(), "Id", "Name", selectedPrinterId);
        ViewBag.Filaments = new MultiSelectList(
            await filamentService.GetAllFilamentsAsync(), "Id", "DisplayName", selectedFilamentIds);
    }
}
