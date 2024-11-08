using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;

namespace ProductSale.Api.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> FetchMessageFromRoom(int roomId)
        {
            var room = _unitOfWork.ChatMessageRepository.Get(c => c.RoomId == roomId).FirstOrDefault();
            if (room == null)
                return new NotFoundObjectResult("Fetch failed.");

            return new OkObjectResult(room);
        }

        public async Task<IActionResult> SendMessageToRoom(ChatMessageDTO model)
        {
            try
            {
                var chatMessage = new ChatMessage
                {
                    Message = model.Message,
                    RoomId = model.RoomId,
                    UserId = model.UserId,
                    SentAt = DateTime.Now,
                };

                _unitOfWork.ChatMessageRepository.Insert(chatMessage);
                _unitOfWork.Save();

                return new OkObjectResult("Send message success.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
