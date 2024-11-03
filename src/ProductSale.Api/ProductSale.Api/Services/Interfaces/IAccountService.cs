using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseDTO> register(Register register);
        Task<ResponseDTO> Login(string userName, string password);
        Task<ResponseDTO> GetUserByUserNameOrEmail(FieldType type, string content);
    }
}
