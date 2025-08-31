using DealershipManagement.Models;

namespace DealershipManagement.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(string id);
        Task<Car?> GetByVINAsync(string vin);
        Task<Car?> GetByStockNumberAsync(string stockNumber);
        Task<IEnumerable<Car>> SearchAsync(SearchFilters filters);
        Task<Car> CreateAsync(Car car);
        Task<bool> UpdateAsync(Car car);
        Task<bool> DeleteAsync(string id);
        Task<long> GetCountAsync(SearchFilters? filters = null);
    }
}
