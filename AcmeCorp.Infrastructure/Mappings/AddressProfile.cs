using AcmeCorp.Domain.Dtos.Address;
using AcmeCorp.Domain.Models;
using AutoMapper;

namespace AcmeCorp.Infrastructure.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressQueryDto>();

            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>();
        }
    }
}
