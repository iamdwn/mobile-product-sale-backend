using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> AddToCart(int productId, int cartId)
        {
            var product = _unitOfWork.ProductRepository.GetByID(productId);
            if (product == null)
                return new NotFoundObjectResult("Product not found.");

            var cart = _unitOfWork.CartRepository.Get(filter: c => c.CartId == cartId, includeProperties: "CartItems").FirstOrDefault();
            if (cart == null)
                return new NotFoundObjectResult("Cart not found.");

            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.ProductId,
                    Price = product.Price,
                    CartId = cart.CartId,
                    Quantity = 1
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            _unitOfWork.CartRepository.Update(cart);
            _unitOfWork.Save();
            return new OkObjectResult("Product added to cart.");
        }

        public async Task<IActionResult> GetCart(int id)
        {
            var cart = _unitOfWork.CartRepository.Get(filter: c => c.CartId == id, includeProperties: "CartItems");
            if (cart == null)
                return new NotFoundObjectResult("Cart not found.");

            return new OkObjectResult(cart);
        }

        public async Task<IActionResult> GetCartByUser(int userId)
        {
            var cart = _unitOfWork.CartRepository.Get(c => c.UserId == userId).FirstOrDefault();
            if (cart == null)
                return new NotFoundObjectResult("User's cart not found.");

            return new OkObjectResult(cart);
        }

        public async Task<IActionResult> RemoveFromCart(int productId, int cartId)
        {
            var cart = _unitOfWork.CartRepository.Get(filter: c => c.CartId == cartId, includeProperties: "CartItems").FirstOrDefault();
            if (cart == null)
                return new NotFoundObjectResult("Cart not found.");

            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
                return new NotFoundObjectResult("Product not found in cart.");

            cart.CartItems.Remove(cartItem);

            _unitOfWork.CartRepository.Update(cart);
            _unitOfWork.Save();
            return new OkObjectResult("Product removed from cart.");
        }
    }
}
