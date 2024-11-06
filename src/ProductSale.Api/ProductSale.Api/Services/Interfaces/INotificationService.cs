using ProductSale.Data.DTO.ResponseModel;

namespace ProductSale.Api.Services.Interfaces
{
    public interface INotificationService
    {
        //public Task<ResponseDTO> getAllNotification(Notification notification);
        //public Task<ResponseDTO> createNotification(Noti);
        //public Task<ResponseDTO> createNotification(Noti);
        public Task<ResponseDTO> removeNotification(List<int> notify);

    }
}
