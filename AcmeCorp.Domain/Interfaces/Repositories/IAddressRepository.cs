using AcmeCorp.Domain.Models;

namespace AcmeCorp.Domain.Interfaces.Repositories
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<List<Address>> GetByCustomerIdAsync(int customerId);
    }
}
