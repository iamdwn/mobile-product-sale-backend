namespace ProductSale.Api.Services.Interfaces
{
    public interface IGoMapsProService
    {
        Task<string> GetLocationDataAsync(string query);
    }
}
