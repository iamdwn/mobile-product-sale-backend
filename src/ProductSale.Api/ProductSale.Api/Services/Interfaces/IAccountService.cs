using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseDTO> getUserByUserName(string userName);
        Task<ResponseDTO> register(Register register);
        Task<ResponseDTO> Login(string userName, string password);
    }
}
