using Data_PrintFarm_Manager;
using Data_PrintFarm_Manager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintFarm_Manager.ViewModels.Printer;

namespace PrintFarm_Manager.Controllers;

public class PrinterController : Controller
{
    private readonly PrintFarmDbContext context;

    public PrinterController(PrintFarmDbContext context)
    {
        this.context = context;
    }

    public async Task<IActionResult> Index()
    {
        var printers = await context.Printers
            .Include(p => p.PrintJobs)
            .OrderBy(p => p.Name)
            .ToListAsync();

        var model = printers.Select(p => new PrinterIndexViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Model = p.Model,
            Location = p.Location,
            IsOperational = p.IsOperational,
            PrintJobsCount = p.PrintJobs.Count
        }).ToList();

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var printer = await context.Printers
            .Include(p => p.PrintJobs)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (printer == null)
            return NotFound();

        var model = new PrinterDetailsViewModel
        {
            Id = printer.Id,
            Name = printer.Name,
            Model = printer.Model,
            Location = printer.Location,
            IsOperational = printer.IsOperational,
            PrintJobs = printer.PrintJobs
                .OrderByDescending(j => j.CreatedOn)
                .Select(j => $"{j.Title} ({j.Material})")
                .ToList()
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View(new PrinterCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrinterCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var printer = new Printer
        {
            Name = viewModel.Name,
            Model = viewModel.Model,
            Location = viewModel.Location,
            IsOperational = viewModel.IsOperational
        };

        context.Printers.Add(printer);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var printer = await context.Printers.FindAsync(id);
        if (printer == null)
            return NotFound();

        var model = new PrinterEditViewModel
        {
            Id = printer.Id,
            Name = printer.Name,
            Model = printer.Model,
            Location = printer.Location,
            IsOperational = printer.IsOperational
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PrinterEditViewModel viewModel)
    {
        if (id != viewModel.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(viewModel);

        var printer = await context.Printers.FindAsync(id);
        if (printer == null)
            return NotFound();

        printer.Name = viewModel.Name;
        printer.Model = viewModel.Model;
        printer.Location = viewModel.Location;
        printer.IsOperational = viewModel.IsOperational;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var printer = await context.Printers
            .Include(p => p.PrintJobs)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (printer == null)
            return NotFound();

        var model = new PrinterDeleteViewModel
        {
            Id = printer.Id,
            Name = printer.Name,
            PrintJobsCount = printer.PrintJobs.Count
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var printer = await context.Printers.FindAsync(id);
        if (printer != null)
        {
            context.Printers.Remove(printer);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
