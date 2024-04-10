using AcmeCorp.Domain.Dtos.Customer;
using AcmeCorp.Domain.Models;

namespace AcmeCorp.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<CustomerQueryDto> GetByIdAsync(int id);
        Task<List<CustomerQueryDto>> GetAllAsync(bool includeArchived);
        Task<CustomerQueryDto> CreateAsync(CustomerCreateDto customer);
        Task UpdateAsync(CustomerUpdateDto customer);
    }
}
