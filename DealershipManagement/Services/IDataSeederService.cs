namespace DealershipManagement.Services
{
    public interface IDataSeederService
    {
        Task SeedSampleDataAsync();
        Task<bool> HasDataAsync();
    }
}
