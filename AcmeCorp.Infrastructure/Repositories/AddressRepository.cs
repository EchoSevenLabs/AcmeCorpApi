using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorp.Infrastructure.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(AcmeDbContext context) : base(context) { }

        public async Task<List<Address>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Addresses.Where(e => e.CustomerId == customerId).ToListAsync();
        }
    }
}
