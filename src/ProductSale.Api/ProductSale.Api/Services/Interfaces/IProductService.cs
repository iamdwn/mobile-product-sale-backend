using Microsoft.AspNetCore.Mvc;
using ProductSale.Data.Models;
namespace ProductSale.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<IActionResult> GetProducts();
        Task<IActionResult> GetProduct(int id);
    }
}
