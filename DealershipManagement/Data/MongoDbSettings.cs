namespace DealershipManagement.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CarsCollectionName { get; set; } = "Cars";
        public string InvoicesCollectionName { get; set; } = "Invoices";
    }
}
