using DealershipManagement.Models;
using DealershipManagement.Repositories;

namespace DealershipManagement.Services
{
    public class DataSeederService : IDataSeederService
    {
        private readonly ICarRepository _carRepository;

        public DataSeederService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<bool> HasDataAsync()
        {
            var count = await _carRepository.GetCountAsync();
            return count > 0;
        }

        public async Task SeedSampleDataAsync()
        {
            if (await HasDataAsync())
                return;

            var sampleCars = new List<Car>
            {
                new Car
                {
                    VIN = "1HGBH41JXMN109186",
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2021,
                    StockNumber = "HON001",
                    BuyPrice = 15000.00m,
                    BuyDate = DateTime.Today.AddDays(-30),
                    MaintenanceLogs = new List<MaintenanceLog>
                    {
                        new MaintenanceLog
                        {
                            PartNumber = "BP001",
                            PartDescription = "Brake Pads",
                            Price = 150.00m,
                            Date = DateTime.Today.AddDays(-15),
                            InvoiceFilePath = ""
                        },
                        new MaintenanceLog
                        {
                            PartNumber = "OF001",
                            PartDescription = "Oil Filter Change",
                            Price = 45.00m,
                            Date = DateTime.Today.AddDays(-10),
                            InvoiceFilePath = ""
                        }
                    }
                },
                new Car
                {
                    VIN = "2T1BURHE0JC123456",
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2020,
                    StockNumber = "TOY001",
                    BuyPrice = 18000.00m,
                    BuyDate = DateTime.Today.AddDays(-45),
                    SellPrice = 22000.00m,
                    SellDate = DateTime.Today.AddDays(-10),
                    MaintenanceLogs = new List<MaintenanceLog>
                    {
                        new MaintenanceLog
                        {
                            PartNumber = "TS001",
                            PartDescription = "Tire Set",
                            Price = 400.00m,
                            Date = DateTime.Today.AddDays(-20),
                            InvoiceFilePath = ""
                        }
                    }
                },
                new Car
                {
                    VIN = "3VWDX7AJ5DM123789",
                    Make = "Volkswagen",
                    Model = "Jetta",
                    Year = 2022,
                    StockNumber = "VW001",
                    BuyPrice = 20000.00m,
                    BuyDate = DateTime.Today.AddDays(-20),
                    MaintenanceLogs = new List<MaintenanceLog>
                    {
                        new MaintenanceLog
                        {
                            PartNumber = "AC001",
                            PartDescription = "Air Filter",
                            Price = 35.00m,
                            Date = DateTime.Today.AddDays(-5),
                            InvoiceFilePath = ""
                        }
                    }
                },
                new Car
                {
                    VIN = "4T1B11HK5JU456789",
                    Make = "Toyota",
                    Model = "Corolla",
                    Year = 2021,
                    StockNumber = "TOY002",
                    BuyPrice = 16000.00m,
                    BuyDate = DateTime.Today.AddDays(-60),
                    SellPrice = 19500.00m,
                    SellDate = DateTime.Today.AddDays(-5),
                    MaintenanceLogs = new List<MaintenanceLog>()
                },
                new Car
                {
                    VIN = "5NPE34AF2FH789012",
                    Make = "Hyundai",
                    Model = "Elantra",
                    Year = 2022,
                    StockNumber = "HYU001",
                    BuyPrice = 17000.00m,
                    BuyDate = DateTime.Today.AddDays(-15),
                    MaintenanceLogs = new List<MaintenanceLog>()
                }
            };

            foreach (var car in sampleCars)
            {
                await _carRepository.CreateAsync(car);
            }
        }
    }
}
