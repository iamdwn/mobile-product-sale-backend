using Microsoft.AspNetCore.Mvc;
using ProductSale.Data.DTO.RequestModel;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IChatService
    {
        Task<IActionResult> SendMessageToRoom(ChatMessageDTO model);
        Task<IActionResult> FetchMessageFromRoom(int roomId);
    }
}
