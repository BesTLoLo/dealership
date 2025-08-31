namespace DealershipManagement.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "DealershipDB";
        public string CarsCollectionName { get; set; } = "Cars";
        public string InvoicesCollectionName { get; set; } = "Invoices";
    }
}
