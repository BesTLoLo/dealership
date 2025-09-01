using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.ComponentModel.DataAnnotations;

namespace DealershipManagement.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; } = "DealershipDB";
        public string CarsCollectionName { get; set; } = "Cars";
        public string InvoicesCollectionName { get; set; } = "Invoices";

        public MongoDbSettings()
        {
            ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
                               ?? "mongodb://localhost:27017";
        }
    }
}
