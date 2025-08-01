using AutoMapper;
using BackendProject.Dto;
using BackendProject.Models;

namespace BackendProject.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserResponseDto>();
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductWithCategoryDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<CatAddDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CartItem, CartViewDto>()
                .ForMember(dest => dest.ProductName,
                           opt => opt.MapFrom(src => src.Product.ProductName));
            CreateMap<WishList, WishListViewDto>();
                
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<Order, OrderViewDto>()
               .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
               .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => src.DeliveryStatus))
               .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus));
            CreateMap<AddNewAddressDto, UserAddress>();
            CreateMap<UserAddress, GetAddressDto>();
            CreateMap<Order, CreateOrderDto>()
                .ForMember(dest => dest.razorpay_order_id,
                           opt => opt.MapFrom(src => src.razorpay_order_id));
        }
    }
}
