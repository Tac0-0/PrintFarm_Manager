using Data_PrintFarm_Manager.Entities;

namespace PrintFarm_Manager_Core.Interfaces;

public interface IFilamentService
{
    Task<List<Filament>> GetAllFilamentsAsync();
    Task<Filament?> GetFilamentByIdAsync(int id);
    Task<Filament?> GetFilamentDetailsAsync(int id);
    Task<bool> NameExistsAsync(string name, string color, int? excludingId = null);
    Task AddFilamentAsync(Filament filament);
    Task<bool> UpdateFilamentAsync(Filament filament);
    Task<bool> DeleteFilamentAsync(int id);
}
