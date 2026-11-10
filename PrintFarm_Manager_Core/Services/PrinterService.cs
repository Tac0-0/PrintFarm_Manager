using Data_PrintFarm_Manager;
using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using PrintFarm_Manager_Core.Interfaces;

namespace PrintFarm_Manager_Core.Services;

public class PrinterService : IPrinterService
{
    private readonly PrintFarmDbContext context;

    public PrinterService(PrintFarmDbContext context) => this.context = context;

    public Task<List<Printer>> GetAllPrintersAsync(
        string? search = null, bool? isOperational = null, string? sortOrder = null)
    {
        var query = context.Printers.AsNoTracking()
            .Include(p => p.PrintJobs)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchText = search.Trim();
            query = query.Where(p =>
                p.Name.Contains(searchText) ||
                p.Model.Contains(searchText) ||
                p.Location.Contains(searchText));
        }

        if (isOperational.HasValue)
        {
            query = query.Where(p => p.IsOperational == isOperational.Value);
        }

        query = sortOrder switch
        {
            "name_desc" => query.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
            "model" => query.OrderBy(p => p.Model).ThenBy(p => p.Name),
            "jobs_desc" => query.OrderByDescending(p => p.PrintJobs.Count).ThenBy(p => p.Name),
            _ => query.OrderBy(p => p.Name).ThenBy(p => p.Id)
        };

        return query.ToListAsync();
    }

    public Task<Printer?> GetPrinterByIdAsync(int id) =>
        context.Printers.AsNoTracking()
            .Include(p => p.PrintJobs)
            .FirstOrDefaultAsync(p => p.Id == id);

    public Task<Printer?> GetPrinterDetailsAsync(int id) =>
        GetPrinterByIdAsync(id);

    public async Task AddPrinterAsync(Printer printer)
    {
        context.Printers.Add(printer);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdatePrinterAsync(Printer printer)
    {
        var existing = await context.Printers.FindAsync(printer.Id);
        if (existing == null) return false;

        existing.Name = printer.Name;
        existing.Model = printer.Model;
        existing.Location = printer.Location;
        existing.IsOperational = printer.IsOperational;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePrinterAsync(int id)
    {
        var printer = await context.Printers.FindAsync(id);
        if (printer == null) return false;

        context.Printers.Remove(printer);
        await context.SaveChangesAsync();
        return true;
    }
}
