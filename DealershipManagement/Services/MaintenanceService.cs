using DealershipManagement.Models;
using DealershipManagement.Repositories;

namespace DealershipManagement.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ICarRepository _carRepository;
        private const decimal TAX_RATE = 0.13m; // 13% tax rate

        public MaintenanceService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<bool> AddMaintenanceLogAsync(string carId, MaintenanceLog log)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return false;

            // Calculate final price with tax
            log.FinalPrice = log.Price + (log.Price * TAX_RATE);
            
            // Set date if not provided
            if (log.Date == default)
                log.Date = DateTime.UtcNow;

            car.MaintenanceLogs.Add(log);
            return await _carRepository.UpdateAsync(car);
        }

        public async Task<bool> UpdateMaintenanceLogAsync(string carId, MaintenanceLog log)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return false;

            var existingLog = car.MaintenanceLogs.FirstOrDefault(l => l.Id == log.Id);
            if (existingLog == null)
                return false;

            // Update log properties
            existingLog.PartNumber = log.PartNumber;
            existingLog.PartDescription = log.PartDescription;
            existingLog.Price = log.Price;
            existingLog.Date = log.Date;
            existingLog.InvoiceFilePath = log.InvoiceFilePath;

            // Recalculate final price
            existingLog.FinalPrice = log.Price + (log.Price * TAX_RATE);

            return await _carRepository.UpdateAsync(car);
        }

        public async Task<bool> DeleteMaintenanceLogAsync(string carId, string logId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return false;

            var logToRemove = car.MaintenanceLogs.FirstOrDefault(l => l.Id == logId);
            if (logToRemove == null)
                return false;

            car.MaintenanceLogs.Remove(logToRemove);
            return await _carRepository.UpdateAsync(car);
        }

        public async Task<MaintenanceLog?> GetMaintenanceLogAsync(string carId, string logId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            return car?.MaintenanceLogs.FirstOrDefault(l => l.Id == logId);
        }

        public async Task<IEnumerable<MaintenanceLog>> GetMaintenanceLogsAsync(string carId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            return car?.MaintenanceLogs ?? Enumerable.Empty<MaintenanceLog>();
        }
    }
}
