using AutoMapper;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Api.Services.Mapper;
using ProductSale.Data.Base;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;
using System.Globalization;
using System.Net;

namespace ProductSale.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> createNotification(NewNoti notification)
        {
            var user = await Task.FromResult(_unitOfWork.UserRepository.Get(c => c.UserId == notification.UserId).SingleOrDefault());

            if (user == null)
            {
                return ResponseUtils.Error("Request fails", "User not found", HttpStatusCode.NotFound);
            }

            string format = "dd/MM/yyyy HH:mm";

            Notification newNotification = _mapper.Map<Notification>(notification);

            DateTime expiredDate = DateTime.ParseExact(notification.CreatedAt, format, CultureInfo.InvariantCulture);

            DateTime utcDateTime = expiredDate.ToUniversalTime();

            newNotification.CreatedAt = utcDateTime;

            _unitOfWork.NotificationRepository.Insert(newNotification);
            _unitOfWork.Save();

            return ResponseUtils.GetObject(newNotification, "Notification created successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> getAllNotification(int userId)
        {
            var user = await Task.FromResult(_unitOfWork.UserRepository.Get(c => c.UserId == userId).SingleOrDefault());

            if (user == null)
            {
                return ResponseUtils.Error("Request fails", "User not found", HttpStatusCode.NotFound);
            }

            var notifications = await Task.FromResult(_unitOfWork.NotificationRepository.Get(c => c.UserId == userId));

            List<NotificationDTO> notificationDTO = new List<NotificationDTO>();

            foreach (var item in notifications)
            {
                notificationDTO.Add(_mapper.Map<NotificationDTO>(item));
            }

            return ResponseUtils.GetObject(notificationDTO, "Notification retrieved successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> isReadNotification(int notificationId)
        {
            var notification = await Task.FromResult(_unitOfWork.NotificationRepository.Get(c => c.NotificationId == notificationId).SingleOrDefault());

            if (notification == null)
            {
                return ResponseUtils.Error("Request fails", "Notification not found", HttpStatusCode.NotFound);
            }

            notification.IsRead = true;

            _unitOfWork.NotificationRepository.Update(notification);
            _unitOfWork.Save();
            return ResponseUtils.GetObject("Request accepted", "Notification updated successfully", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> removeNotification(List<int> notify)
        {
            foreach (var item in notify)
            {
                var notification = await Task.FromResult(_unitOfWork.NotificationRepository.Get(c => c.NotificationId == item).SingleOrDefault());

                if (notification == null)
                {
                    return ResponseUtils.Error("Request fails", "Notification not found", HttpStatusCode.NotFound);
                }
                _unitOfWork.NotificationRepository.Delete(notification);
                _unitOfWork.Save();
            }

            return ResponseUtils.GetObject("Request accepted", "Notification removed successfully", HttpStatusCode.OK, null);
        }
    }
}
