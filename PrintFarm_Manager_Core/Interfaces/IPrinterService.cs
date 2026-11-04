using Data_PrintFarm_Manager.Entities;

namespace PrintFarm_Manager_Core.Interfaces;

public interface IPrinterService
{
    Task<List<Printer>> GetAllPrintersAsync();
    Task<Printer?> GetPrinterByIdAsync(int id);
    Task<Printer?> GetPrinterDetailsAsync(int id);
    Task AddPrinterAsync(Printer printer);
    Task<bool> UpdatePrinterAsync(Printer printer);
    Task<bool> DeletePrinterAsync(int id);
}
