using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorp.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AcmeDbContext context) : base(context) { }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders.Where(e => e.CustomerId == customerId).ToListAsync();
        }
    }
}
