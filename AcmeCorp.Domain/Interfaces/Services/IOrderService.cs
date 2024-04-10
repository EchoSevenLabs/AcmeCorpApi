using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Models;

namespace AcmeCorp.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderQueryDto> GetByIdAsync(int id);
        Task<List<OrderQueryDto>> GetByCustomerIdAsync(int customerId);
        Task<List<OrderQueryDto>> GetAllAsync(bool includeArchived);
        Task<OrderQueryDto> CreateAsync(OrderCreateDto customer);
        Task UpdateAsync(OrderUpdateDto customer);
    }
}
