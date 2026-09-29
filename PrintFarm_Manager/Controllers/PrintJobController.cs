using Data_PrintFarm_Manager;
using Data_PrintFarm_Manager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrintFarm_Manager.ViewModels.PrintJob;

namespace PrintFarm_Manager.Controllers;

public class PrintJobController : Controller
{
    private readonly PrintFarmDbContext context;

    public PrintJobController(PrintFarmDbContext context)
    {
        this.context = context;
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await context.PrintJobs
            .Include(j => j.Printer)
            .OrderByDescending(j => j.CreatedOn)
            .ToListAsync();

        var model = jobs.Select(j => new PrintJobIndexViewModel
        {
            Id = j.Id,
            Title = j.Title,
            Material = j.Material,
            EstimatedHours = j.EstimatedHours,
            PrinterName = j.Printer.Name
        }).ToList();

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var job = await context.PrintJobs
            .Include(j => j.Printer)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            return NotFound();

        var model = new PrintJobDetailsViewModel
        {
            Id = job.Id,
            Title = job.Title,
            Material = job.Material,
            EstimatedHours = job.EstimatedHours,
            CreatedOn = job.CreatedOn,
            PrinterName = job.Printer.Name,
            PrinterLocation = job.Printer.Location
        };

        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        await PopulatePrinters();
        return View(new PrintJobCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrintJobCreateViewModel model)
    {
        if (!await context.Printers.AnyAsync(p => p.Id == model.PrinterId))
            ModelState.AddModelError(nameof(model.PrinterId), "Избраният принтер не съществува.");

        if (!ModelState.IsValid)
        {
            await PopulatePrinters(model.PrinterId);
            return View(model);
        }

        var job = new PrintJob
        {
            Title = model.Title,
            Material = model.Material,
            EstimatedHours = model.EstimatedHours,
            CreatedOn = DateTime.Now,
            PrinterId = model.PrinterId
        };

        context.PrintJobs.Add(job);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var job = await context.PrintJobs.FindAsync(id);
        if (job == null)
            return NotFound();

        var model = new PrintJobEditViewModel
        {
            Id = job.Id,
            Title = job.Title,
            Material = job.Material,
            EstimatedHours = job.EstimatedHours,
            PrinterId = job.PrinterId
        };

        await PopulatePrinters(job.PrinterId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PrintJobEditViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (!await context.Printers.AnyAsync(p => p.Id == model.PrinterId))
            ModelState.AddModelError(nameof(model.PrinterId), "Избраният принтер не съществува.");

        if (!ModelState.IsValid)
        {
            await PopulatePrinters(model.PrinterId);
            return View(model);
        }

        var job = await context.PrintJobs.FindAsync(id);
        if (job == null)
            return NotFound();

        job.Title = model.Title;
        job.Material = model.Material;
        job.EstimatedHours = model.EstimatedHours;
        job.PrinterId = model.PrinterId;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var job = await context.PrintJobs
            .Include(j => j.Printer)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            return NotFound();

        var model = new PrintJobDeleteViewModel
        {
            Id = job.Id,
            Title = job.Title,
            PrinterName = job.Printer.Name
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var job = await context.PrintJobs.FindAsync(id);
        if (job != null)
        {
            context.PrintJobs.Remove(job);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulatePrinters(int? selectedPrinterId = null)
    {
        var printers = await context.Printers
            .OrderBy(p => p.Name)
            .ToListAsync();

        ViewBag.Printers = new SelectList(printers, "Id", "Name", selectedPrinterId);
    }
}
