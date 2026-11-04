using Data_PrintFarm_Manager;
using Data_PrintFarm_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using PrintFarm_Manager_Core.Interfaces;

namespace PrintFarm_Manager_Core.Services;

public class FilamentService : IFilamentService
{
    private readonly PrintFarmDbContext context;

    public FilamentService(PrintFarmDbContext context) => this.context = context;

    public Task<List<Filament>> GetAllFilamentsAsync() =>
        context.Filaments.AsNoTracking()
            .Include(f => f.PrintJobFilaments)
            .OrderBy(f => f.Name).ThenBy(f => f.Color)
            .ToListAsync();

    public Task<Filament?> GetFilamentByIdAsync(int id) =>
        context.Filaments.AsNoTracking()
            .Include(f => f.PrintJobFilaments)
            .FirstOrDefaultAsync(f => f.Id == id);

    public Task<Filament?> GetFilamentDetailsAsync(int id) =>
        context.Filaments.AsNoTracking()
            .Include(f => f.PrintJobFilaments).ThenInclude(link => link.PrintJob)
            .FirstOrDefaultAsync(f => f.Id == id);

    public Task<bool> NameExistsAsync(string name, string color, int? excludingId = null) =>
        context.Filaments.AnyAsync(f => f.Name == name && f.Color == color && (!excludingId.HasValue || f.Id != excludingId.Value));

    public async Task AddFilamentAsync(Filament filament)
    {
        context.Filaments.Add(filament);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdateFilamentAsync(Filament updated)
    {
        var existing = await context.Filaments.FindAsync(updated.Id);
        if (existing == null) return false;

        existing.Name = updated.Name;
        existing.Color = updated.Color;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFilamentAsync(int id)
    {
        var filament = await context.Filaments.FindAsync(id);
        if (filament == null) return false;

        context.Filaments.Remove(filament);
        await context.SaveChangesAsync();
        return true;
    }
}
