
using AutoMapper;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;
namespace ProductSale.Api.Services.Mapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<User, AccountDTO>();
            CreateMap<AccountDTO, User>();
            CreateMap<Register, User>();
            CreateMap<NotificationDTO, Notification>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<NewNoti, Notification>();
        }
    }
}
