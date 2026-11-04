using Data_PrintFarm_Manager;
using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using PrintFarm_Manager_Core.Interfaces;

namespace PrintFarm_Manager_Core.Services;

public class PrintJobService : IPrintJobService
{
    private readonly PrintFarmDbContext context;

    public PrintJobService(PrintFarmDbContext context) => this.context = context;

    public Task<List<PrintJob>> GetAllPrintJobsAsync() =>
        context.PrintJobs.AsNoTracking()
            .Include(j => j.Printer)
            .Include(j => j.PrintJobFilaments).ThenInclude(link => link.Filament)
            .OrderByDescending(j => j.CreatedOn)
            .ToListAsync();

    public Task<PrintJob?> GetPrintJobByIdAsync(int id) =>
        context.PrintJobs.AsNoTracking()
            .Include(j => j.PrintJobFilaments).ThenInclude(link => link.Filament)
            .FirstOrDefaultAsync(j => j.Id == id);

    public Task<PrintJob?> GetPrintJobDetailsAsync(int id) =>
        context.PrintJobs.AsNoTracking()
            .Include(j => j.Printer)
            .Include(j => j.PrintJobFilaments).ThenInclude(link => link.Filament)
            .FirstOrDefaultAsync(j => j.Id == id);

    public async Task AddPrintJobAsync(PrintJob job, IEnumerable<int> filamentIds)
    {
        foreach (var filamentId in filamentIds.Distinct())
            job.PrintJobFilaments.Add(new PrintJobFilament { FilamentId = filamentId });

        context.PrintJobs.Add(job);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdatePrintJobAsync(PrintJob updated, IEnumerable<int> filamentIds)
    {
        var job = await context.PrintJobs
            .Include(j => j.PrintJobFilaments)
            .FirstOrDefaultAsync(j => j.Id == updated.Id);
        if (job == null) return false;

        job.Title = updated.Title;
        job.EstimatedHours = updated.EstimatedHours;
        job.PrinterId = updated.PrinterId;

        var selected = filamentIds.Distinct().ToHashSet();
        var existingIds = job.PrintJobFilaments.Select(link => link.FilamentId).ToHashSet();

        foreach (var link in job.PrintJobFilaments.Where(link => !selected.Contains(link.FilamentId)).ToList())
            context.PrintJobFilaments.Remove(link);

        foreach (var filamentId in selected.Except(existingIds))
            context.PrintJobFilaments.Add(new PrintJobFilament { PrintJobId = job.Id, FilamentId = filamentId });

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePrintJobAsync(int id)
    {
        var job = await context.PrintJobs.FindAsync(id);
        if (job == null) return false;

        context.PrintJobs.Remove(job);
        await context.SaveChangesAsync();
        return true;
    }
}
