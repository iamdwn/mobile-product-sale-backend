using Microsoft.AspNetCore.Mvc;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services.Interfaces
{
    public interface ICartService
    {
        Task<IActionResult> GetCart(int id);
        Task<IActionResult> GetCartByUser(int id);
        Task<IActionResult> AddToCart(int productId, int cart);
        Task<IActionResult> RemoveFromCart(int productId, int cart);
    }
}
