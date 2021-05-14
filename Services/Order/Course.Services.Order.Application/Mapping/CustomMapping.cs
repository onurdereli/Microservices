using AutoMapper;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Domain.OrderAggregate;

namespace Course.Services.Order.Application.Mapping
{
    public class CustomMapping: Profile
    {
        public CustomMapping()
        {
            CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItem>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
