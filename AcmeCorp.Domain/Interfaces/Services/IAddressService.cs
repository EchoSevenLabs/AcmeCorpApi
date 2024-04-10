using AcmeCorp.Domain.Dtos.Address;

namespace AcmeCorp.Domain.Interfaces.Services
{
    public interface IAddressService
    {
        Task<AddressQueryDto> GetByIdAsync(int id);
        Task<List<AddressQueryDto>> GetByCustomerIdAsync(int customerId);
        Task<List<AddressQueryDto>> GetAllAsync(bool includeArchived);
        Task<AddressQueryDto> CreateAsync(AddressCreateDto customer);
        Task UpdateAsync(AddressUpdateDto customer);
    }
}
