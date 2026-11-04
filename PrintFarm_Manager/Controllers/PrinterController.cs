using Data_PrintFarm_Manager.Entities;
using Microsoft.AspNetCore.Mvc;
using PrintFarm_Manager.ViewModels.Printer;
using PrintFarm_Manager_Core.Interfaces;

namespace PrintFarm_Manager.Controllers;

public class PrinterController : Controller
{
    private readonly IPrinterService printerService;

    public PrinterController(IPrinterService printerService) => this.printerService = printerService;

    public async Task<IActionResult> Index()
    {
        var printers = await printerService.GetAllPrintersAsync();
        return View(printers.Select(p => new PrinterIndexViewModel
        {
            Id = p.Id, Name = p.Name, Model = p.Model, Location = p.Location,
            IsOperational = p.IsOperational, PrintJobsCount = p.PrintJobs.Count
        }).ToList());
    }

    public async Task<IActionResult> Details(int id)
    {
        var printer = await printerService.GetPrinterDetailsAsync(id);
        if (printer == null) return NotFound();

        return View(new PrinterDetailsViewModel
        {
            Id = printer.Id, Name = printer.Name, Model = printer.Model,
            Location = printer.Location, IsOperational = printer.IsOperational,
            PrintJobs = printer.PrintJobs.OrderByDescending(j => j.CreatedOn)
                .Select(j => j.Title).ToList()
        });
    }

    public IActionResult Create() => View(new PrinterCreateViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrinterCreateViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);

        await printerService.AddPrinterAsync(new Printer
        {
            Name = viewModel.Name, Model = viewModel.Model,
            Location = viewModel.Location, IsOperational = viewModel.IsOperational
        });
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var printer = await printerService.GetPrinterByIdAsync(id);
        if (printer == null) return NotFound();

        return View(new PrinterEditViewModel
        {
            Id = printer.Id, Name = printer.Name, Model = printer.Model,
            Location = printer.Location, IsOperational = printer.IsOperational
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PrinterEditViewModel viewModel)
    {
        if (id != viewModel.Id) return NotFound();
        if (!ModelState.IsValid) return View(viewModel);

        var updated = await printerService.UpdatePrinterAsync(new Printer
        {
            Id = id, Name = viewModel.Name, Model = viewModel.Model,
            Location = viewModel.Location, IsOperational = viewModel.IsOperational
        });
        return updated ? RedirectToAction(nameof(Index)) : NotFound();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var printer = await printerService.GetPrinterByIdAsync(id);
        if (printer == null) return NotFound();

        return View(new PrinterDeleteViewModel
        {
            Id = printer.Id, Name = printer.Name, PrintJobsCount = printer.PrintJobs.Count
        });
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await printerService.DeletePrinterAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
