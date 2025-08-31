using DealershipManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DealershipManagement.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Car> _carsCollection;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
            _carsCollection = _database.GetCollection<Car>(settings.Value.CarsCollectionName);
        }

        public IMongoCollection<Car> Cars => _carsCollection;
        public IMongoDatabase Database => _database;
    }
}
