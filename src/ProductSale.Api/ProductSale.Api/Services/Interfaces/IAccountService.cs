using ProductSale.Data.Models;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<User> getUserByUserName(string userName);
    }
}
