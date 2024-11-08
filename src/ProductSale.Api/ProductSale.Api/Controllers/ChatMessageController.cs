using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.DTO.RequestModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatMessageController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("{roomId}")]
        public Task<IActionResult> FetchMessageFromRoom([FromRoute] int roomId)
        {
            return _chatService.FetchMessageFromRoom(roomId);
        }

        [HttpPost]
        public Task<IActionResult> SendMessageToRoom([FromBody] ChatMessageDTO model)
        {
            return _chatService.SendMessageToRoom(model);
        }
    }
}
