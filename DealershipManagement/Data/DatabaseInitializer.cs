using DealershipManagement.Models;
using MongoDB.Driver;

namespace DealershipManagement.Data
{
    public class DatabaseInitializer
    {
        private readonly MongoDbContext _context;

        public DatabaseInitializer(MongoDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Create indexes for better performance
                var indexKeysDefinition = Builders<Car>.IndexKeys
                    .Ascending(c => c.VIN)
                    .Ascending(c => c.StockNumber)
                    .Ascending(c => c.Make)
                    .Ascending(c => c.Model)
                    .Ascending(c => c.Year)
                    .Ascending(c => c.BuyDate)
                    .Ascending(c => c.SellDate);

                var indexOptions = new CreateIndexOptions
                {
                    Unique = false,
                    Name = "CarSearchIndex"
                };

                var indexModel = new CreateIndexModel<Car>(indexKeysDefinition, indexOptions);

                await _context.Cars.Indexes.CreateOneAsync(indexModel);

                // Create unique index on VIN
                var vinIndexKeys = Builders<Car>.IndexKeys.Ascending(c => c.VIN);
                var vinIndexOptions = new CreateIndexOptions
                {
                    Unique = true,
                    Name = "VIN_Unique_Index"
                };
                var vinIndexModel = new CreateIndexModel<Car>(vinIndexKeys, vinIndexOptions);

                await _context.Cars.Indexes.CreateOneAsync(vinIndexModel);

                // Create unique index on StockNumber
                var stockIndexKeys = Builders<Car>.IndexKeys.Ascending(c => c.StockNumber);
                var stockIndexOptions = new CreateIndexOptions
                {
                    Unique = true,
                    Name = "StockNumber_Unique_Index"
                };
                var stockIndexModel = new CreateIndexModel<Car>(stockIndexKeys, stockIndexOptions);

                await _context.Cars.Indexes.CreateOneAsync(stockIndexModel);

                Console.WriteLine("Database indexes created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database indexes: {ex.Message}");
            }
        }
    }
}
