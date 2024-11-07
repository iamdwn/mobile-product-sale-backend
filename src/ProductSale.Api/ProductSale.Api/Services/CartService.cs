using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;
using ProductSale.Data.Persistences;

namespace ProductSale.Api.Services
{
    public class CartService : ICartService
    {
        private readonly ProductSaleContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, ProductSaleContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
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

            cart.UpdateTotalPrice();
            _unitOfWork.CartRepository.Update(cart);
            _unitOfWork.Save();
            return new OkObjectResult("Product added to cart.");
        }

        public async Task<IActionResult> GetCart(int id)
        {
            var cart = _context.Carts.Where(c => c.CartId == id)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault();
            if (cart == null)
                return new NotFoundObjectResult("Cart not found.");

            var cartDTO = _mapper.Map<CartDTO>(cart);
            return new OkObjectResult(cartDTO);
        }

        public async Task<IActionResult> GetCartByUser(int userId)
        {
            var cart = _context.Carts.Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault();
            if (cart == null)
                return new NotFoundObjectResult("User's cart not found.");

            var cartDTO = _mapper.Map<CartDTO>(cart);
            return new OkObjectResult(cartDTO);
        }

        public async Task<IActionResult> RemoveFromCart(int productId, int cartId)
        {
            var cart = _unitOfWork.CartRepository.Get(filter: c => c.CartId == cartId, includeProperties: "CartItems").FirstOrDefault();
            if (cart == null)
                return new NotFoundObjectResult("Cart not found.");

            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
                return new NotFoundObjectResult("Product not found in cart.");

            if (cartItem.Quantity == 1)
            {
                cart.CartItems.Remove(cartItem);
                _unitOfWork.CartItemRepository.Delete(cartItem);
            }

            cartItem.Quantity--;
            _unitOfWork.CartItemRepository.Update(cartItem);

            cart.UpdateTotalPrice();
            _unitOfWork.CartRepository.Update(cart);
            _unitOfWork.Save();
            return new OkObjectResult("Product removed from cart.");
        }
    }
}
