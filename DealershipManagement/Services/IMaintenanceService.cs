using DealershipManagement.Models;

namespace DealershipManagement.Services
{
    public interface IMaintenanceService
    {
        Task<bool> AddMaintenanceLogAsync(string carId, MaintenanceLog log);
        Task<bool> UpdateMaintenanceLogAsync(string carId, MaintenanceLog log);
        Task<bool> DeleteMaintenanceLogAsync(string carId, string logId);
        Task<MaintenanceLog?> GetMaintenanceLogAsync(string carId, string logId);
        Task<IEnumerable<MaintenanceLog>> GetMaintenanceLogsAsync(string carId);
    }
}
