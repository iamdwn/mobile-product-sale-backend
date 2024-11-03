using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("user")]
        public async Task<ResponseDTO> GetUserByUserNameOrEmail(FieldType type, string content)
        {
            return await _accountService.GetUserByUserNameOrEmail(type, content);
        } 
        
        [HttpPost("registration")]
        public async Task<ResponseDTO> Register([FromQuery, Required] Register register)
        {
            return await _accountService.register(register);
        }
        
        [HttpPost("authen")]
        public async Task<ResponseDTO> Login([FromQuery, Required] string username, string password)
        {
            return await _accountService.Login(username,password);
        }

    }
}
