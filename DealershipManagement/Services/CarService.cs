using DealershipManagement.Models;
using DealershipManagement.Repositories;

namespace DealershipManagement.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private const decimal TAX_RATE = 0.10m; // 10% tax rate

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car?> GetCarByIdAsync(string id)
        {
            return await _carRepository.GetByIdAsync(id);
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            // Calculate final buy price with tax
            car.FinalBuyPrice = car.BuyPrice + (car.BuyPrice * TAX_RATE);
            car.Tax = car.BuyPrice * TAX_RATE;
            
            // Set buy date if not provided
            if (!car.BuyDate.HasValue)
                car.BuyDate = DateTime.UtcNow;

            return await _carRepository.CreateAsync(car);
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            var existingCar = await _carRepository.GetByIdAsync(car.Id);
            if (existingCar == null)
                return false;

            // Recalculate final buy price if buy price changed
            if (car.BuyPrice != existingCar.BuyPrice)
            {
                car.FinalBuyPrice = car.BuyPrice + (car.BuyPrice * TAX_RATE);
                car.Tax = car.BuyPrice * TAX_RATE;
            }

            // Preserve sell information if it exists
            if (existingCar.SellDate.HasValue)
            {
                car.SellDate = existingCar.SellDate;
                car.SellPrice = existingCar.SellPrice;
                car.FinalSellPrice = existingCar.FinalSellPrice;
            }

            return await _carRepository.UpdateAsync(car);
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            return await _carRepository.DeleteAsync(id);
        }

        public async Task<bool> SellCarAsync(string id, decimal sellPrice, DateTime sellDate)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
                return false;

            car.SellPrice = sellPrice;
            car.SellDate = sellDate;
            car.FinalSellPrice = sellPrice + (sellPrice * TAX_RATE);

            return await _carRepository.UpdateAsync(car);
        }

        public async Task<Car?> GetCarByVINAsync(string vin)
        {
            return await _carRepository.GetByVINAsync(vin);
        }

        public async Task<Car?> GetCarByStockNumberAsync(string stockNumber)
        {
            return await _carRepository.GetByStockNumberAsync(stockNumber);
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(SearchFilters filters)
        {
            return await _carRepository.SearchAsync(filters);
        }

        public async Task<long> GetCarsCountAsync(SearchFilters? filters = null)
        {
            return await _carRepository.GetCountAsync(filters);
        }

        public async Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm)
        {
            return await _carRepository.GetSearchSuggestionsAsync(searchTerm);
        }
    }
}
