using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;

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
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            return Ok(await _accountService.getUserByUserName(userName));
        }

    }
}
