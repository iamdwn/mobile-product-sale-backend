using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Api.Services;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using System.ComponentModel.DataAnnotations;

namespace ProductSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("new")]
        public async Task<ResponseDTO> NewNotification([FromBody] NewNoti noti)
        {
            return await _notificationService.createNotification(noti);
        }

        [HttpGet("get")]
        public async Task<ResponseDTO> GetNotifications([FromQuery] int userId)
        {
            return await _notificationService.getAllNotification(userId);
        }

        [HttpPost("read")]
        public async Task<ResponseDTO> ReadNoti([FromQuery] int notiId)
        {
            return await _notificationService.isReadNotification(notiId);
        }

        [HttpDelete("remove")]
        public async Task<ResponseDTO> RemoveNoti([FromQuery] int notiId)
        {
            return await _notificationService.removeNotification(notiId);
        }


    }
}
