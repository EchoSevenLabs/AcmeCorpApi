using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Models;

namespace AcmeCorp.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
    }
}
