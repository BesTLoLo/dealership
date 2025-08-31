using DealershipManagement.Data;
using DealershipManagement.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DealershipManagement.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Car> _carsCollection;

        public CarRepository(MongoDbContext context)
        {
            _carsCollection = context.Cars;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _carsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(string id)
        {
            return await _carsCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Car?> GetByVINAsync(string vin)
        {
            return await _carsCollection.Find(c => c.VIN == vin).FirstOrDefaultAsync();
        }

        public async Task<Car?> GetByStockNumberAsync(string stockNumber)
        {
            return await _carsCollection.Find(c => c.StockNumber == stockNumber).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Car>> SearchAsync(SearchFilters filters)
        {
            var filter = BuildFilter(filters);
            var sort = Builders<Car>.Sort.Descending(c => c.BuyDate);
            
            return await _carsCollection.Find(filter)
                .Sort(sort)
                .Skip((filters.Page - 1) * filters.PageSize)
                .Limit(filters.PageSize)
                .ToListAsync();
        }

        public async Task<Car> CreateAsync(Car car)
        {
            await _carsCollection.InsertOneAsync(car);
            return car;
        }

        public async Task<bool> UpdateAsync(Car car)
        {
            var result = await _carsCollection.ReplaceOneAsync(c => c.Id == car.Id, car);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _carsCollection.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<long> GetCountAsync(SearchFilters? filters = null)
        {
            if (filters == null)
                return await _carsCollection.CountDocumentsAsync(_ => true);

            var filter = BuildFilter(filters);
            return await _carsCollection.CountDocumentsAsync(filter);
        }

        private FilterDefinition<Car> BuildFilter(SearchFilters filters)
        {
            var builder = Builders<Car>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(filters.SearchTerm))
            {
                var searchFilter = builder.Or(
                    builder.Regex(c => c.VIN, new MongoDB.Bson.BsonRegularExpression(filters.SearchTerm, "i")),
                    builder.Regex(c => c.StockNumber, new MongoDB.Bson.BsonRegularExpression(filters.SearchTerm, "i")),
                    builder.Regex(c => c.Make, new MongoDB.Bson.BsonRegularExpression(filters.SearchTerm, "i")),
                    builder.Regex(c => c.Model, new MongoDB.Bson.BsonRegularExpression(filters.SearchTerm, "i")),
                    builder.Eq(c => c.Year, int.TryParse(filters.SearchTerm, out var year) ? year : 0)
                );
                filter = builder.And(filter, searchFilter);
            }

            if (!string.IsNullOrEmpty(filters.Status))
            {
                if (filters.Status == "Available")
                    filter = builder.And(filter, builder.Eq(c => c.SellDate, null));
                else if (filters.Status == "Sold")
                    filter = builder.And(filter, builder.Ne(c => c.SellDate, null));
            }

            if (!string.IsNullOrEmpty(filters.Make))
                filter = builder.And(filter, builder.Eq(c => c.Make, filters.Make));

            if (!string.IsNullOrEmpty(filters.Model))
                filter = builder.And(filter, builder.Eq(c => c.Model, filters.Model));

            if (filters.Year.HasValue)
                filter = builder.And(filter, builder.Eq(c => c.Year, filters.Year.Value));

            if (filters.StartDate.HasValue)
                filter = builder.And(filter, builder.Gte(c => c.BuyDate, filters.StartDate.Value));

            if (filters.EndDate.HasValue)
                filter = builder.And(filter, builder.Lte(c => c.BuyDate, filters.EndDate.Value));

            return filter;
        }
    }
}
