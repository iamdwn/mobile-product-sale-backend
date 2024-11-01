using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetUserByUserName(string userName)
        {
            return await _accountService.getUserByUserName(userName);
        } 
        
        [HttpPost("registration")]
        public async Task<ResponseDTO> Register(Register register)
        {
            return await _accountService.register(register);
        }
        
        [HttpPost("authen")]
        public async Task<ResponseDTO> Login(string username, string password)
        {
            return await _accountService.Login(username,password);
        }

    }
}
