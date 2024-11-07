
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
            CreateMap<AccountDTO, User>();
            CreateMap<Register, User>();

            CreateMap<Cart, CartDTO>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            // Map CartItem to CartItemDTO
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))  // Assuming Product has ProductName
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
