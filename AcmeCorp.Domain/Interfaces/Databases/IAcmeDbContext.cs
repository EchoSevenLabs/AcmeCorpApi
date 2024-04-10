using AcmeCorp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

namespace AcmeCorp.Domain.Interfaces.Databases
{
    public interface IAcmeDbContext
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders { get; set; }
        string JsonQuery(string column, [NotParameterized] string path);
        string JsonValue(string column, [NotParameterized] string path);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade Database { get; }
    }
}
