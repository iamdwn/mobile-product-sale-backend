using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            return Task.FromResult(_unitOfWork.ProductRepository.Get());
        }
    }
}
