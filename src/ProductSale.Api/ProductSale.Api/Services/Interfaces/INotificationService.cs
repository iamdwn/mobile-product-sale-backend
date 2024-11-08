using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;

namespace ProductSale.Api.Services.Interfaces
{
    public interface INotificationService
    {
        public Task<ResponseDTO> getAllNotification(int userId);
        public Task<ResponseDTO> createNotification(NewNoti notification);
        public Task<ResponseDTO> isReadNotification(int notificationId);
        public Task<ResponseDTO> removeNotification(int notiId);

    }
}
