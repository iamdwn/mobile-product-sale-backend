using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoMapsProController : ControllerBase
    {
        private readonly IGoMapsProService _goMapsProService;

        public GoMapsProController(IGoMapsProService goMapsProService)
        {
            _goMapsProService = goMapsProService;
        }

        [HttpGet("get-location/{query}")]
        public async Task<IActionResult> GetLocation(string query)
        {
            try
            {
                var result = await _goMapsProService.GetLocationDataAsync(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
