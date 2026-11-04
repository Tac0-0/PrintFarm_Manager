using Data_PrintFarm_Manager.Entities;
using Microsoft.AspNetCore.Mvc;
using PrintFarm_Manager.ViewModels.Filament;
using PrintFarm_Manager_Core.Interfaces;

namespace PrintFarm_Manager.Controllers;

public class FilamentController : Controller
{
    private readonly IFilamentService filamentService;

    public FilamentController(IFilamentService filamentService) => this.filamentService = filamentService;

    public async Task<IActionResult> Index()
    {
        var filaments = await filamentService.GetAllFilamentsAsync();
        return View(filaments.Select(f => new FilamentIndexViewModel
        {
            Id = f.Id, Name = f.Name, Color = f.Color,
            PrintJobsCount = f.PrintJobFilaments.Count
        }).ToList());
    }

    public async Task<IActionResult> Details(int id)
    {
        var filament = await filamentService.GetFilamentDetailsAsync(id);
        if (filament == null) return NotFound();

        return View(new FilamentDetailsViewModel
        {
            Id = filament.Id, Name = filament.Name, Color = filament.Color,
            PrintJobs = filament.PrintJobFilaments
                .Select(link => link.PrintJob.Title).OrderBy(name => name).ToList()
        });
    }

    public IActionResult Create() => View(new FilamentCreateViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FilamentCreateViewModel model)
    {
        var name = model.Name?.Trim() ?? string.Empty;
        var color = model.Color?.Trim() ?? string.Empty;
        if (await filamentService.NameExistsAsync(name, color))
            ModelState.AddModelError(nameof(model.Name), "Материал с това име вече съществува.");
        if (!ModelState.IsValid) return View(model);

        await filamentService.AddFilamentAsync(new Filament
        {
            Name = name, Color = color
        });
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var filament = await filamentService.GetFilamentByIdAsync(id);
        if (filament == null) return NotFound();

        return View(new FilamentEditViewModel
        {
            Id = filament.Id, Name = filament.Name, Color = filament.Color
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FilamentEditViewModel model)
    {
        if (id != model.Id) return NotFound();
        var name = model.Name?.Trim() ?? string.Empty;
        var color = model.Color?.Trim() ?? string.Empty;
        if (await filamentService.NameExistsAsync(name, color, id))
            ModelState.AddModelError(nameof(model.Name), "Материал с това име вече съществува.");
        if (!ModelState.IsValid) return View(model);

        var updated = await filamentService.UpdateFilamentAsync(new Filament
        {
            Id = id, Name = name, Color = color
        });
        return updated ? RedirectToAction(nameof(Index)) : NotFound();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var filament = await filamentService.GetFilamentByIdAsync(id);
        if (filament == null) return NotFound();

        return View(new FilamentDeleteViewModel
        {
            Id = filament.Id, Name = filament.Name,
            PrintJobsCount = filament.PrintJobFilaments.Count
        });
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await filamentService.DeleteFilamentAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
