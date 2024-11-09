using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart([FromRoute] int id)
        {
            return await _cartService.GetCart(id);
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetCartByUser([FromRoute] int userId)
        {
            return await _cartService.GetCartByUser(userId);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromQuery] int productId, [FromQuery] int cartId)
        {
            return await _cartService.AddToCart(productId, cartId);
        }

        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int productId, [FromQuery] int cartId)
        {
            return await _cartService.RemoveFromCart(productId, cartId);
        }

        [HttpPut("ClearCart")]
        public async Task<IActionResult> ClearCart([FromQuery] int cartId)
        {
            return await _cartService.ClearCart(cartId);
        }
    }
}
