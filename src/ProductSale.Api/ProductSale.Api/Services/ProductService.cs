using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;

namespace ProductSale.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IActionResult> GetProduct(int id)
        {
            var product = _unitOfWork.ProductRepository.GetByID(id);
            if (product == null)
            {
                return Task.FromResult<IActionResult>(new NotFoundResult());
            }
            return Task.FromResult<IActionResult>(new OkObjectResult(product));
        }

        public Task<IActionResult> GetProducts()
        {
            var products = _unitOfWork.ProductRepository.Get();
            return Task.FromResult<IActionResult>(new OkObjectResult(products));
        }
    }
}
