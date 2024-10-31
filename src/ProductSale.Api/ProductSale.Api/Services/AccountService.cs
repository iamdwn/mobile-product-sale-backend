using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> getUserByUserName(string userName)
        {
            try
            {
                return await Task.FromResult(_unitOfWork.UserRepository.Get(c => c.Username.Equals(userName)).SingleOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
