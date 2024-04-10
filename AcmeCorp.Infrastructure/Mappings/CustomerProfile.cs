using AcmeCorp.Domain.Dtos.Customer;
using AcmeCorp.Domain.Models;
using AutoMapper;

namespace AcmeCorp.Infrastructure.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerQueryDto>();

            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();
        }
    }
}
