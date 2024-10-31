using ProductSale.Data.Models;
namespace ProductSale.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();

    }
}
