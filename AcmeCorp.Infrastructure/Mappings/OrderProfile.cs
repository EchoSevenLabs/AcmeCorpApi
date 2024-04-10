using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Models;
using AutoMapper;

namespace AcmeCorp.Infrastructure.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderQueryDto>();

            CreateMap<OrderCreateDto, Order>();
            CreateMap<OrderUpdateDto, Order>();
        }
    }
}
