using Data_PrintFarm_Manager.Entities;

namespace PrintFarm_Manager_Core.Interfaces;

public interface IPrintJobService
{
    Task<List<PrintJob>> GetAllPrintJobsAsync();
    Task<PrintJob?> GetPrintJobByIdAsync(int id);
    Task<PrintJob?> GetPrintJobDetailsAsync(int id);
    Task AddPrintJobAsync(PrintJob job, IEnumerable<int> filamentIds);
    Task<bool> UpdatePrintJobAsync(PrintJob job, IEnumerable<int> filamentIds);
    Task<bool> DeletePrintJobAsync(int id);
}
