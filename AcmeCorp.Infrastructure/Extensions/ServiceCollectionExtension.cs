using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Infrastructure.Repositories;
using AcmeCorp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeCorp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
