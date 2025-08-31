using DealershipManagement.Models;

namespace DealershipManagement.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(string id);
        Task<Car> AddCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(string id);
        Task<bool> SellCarAsync(string id, decimal sellPrice, DateTime sellDate);
        Task<Car?> GetCarByVINAsync(string vin);
        Task<Car?> GetCarByStockNumberAsync(string stockNumber);
        Task<IEnumerable<Car>> SearchCarsAsync(SearchFilters filters);
        Task<long> GetCarsCountAsync(SearchFilters? filters = null);
    }
}
