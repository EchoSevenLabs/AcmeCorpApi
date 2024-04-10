using AcmeCorp.Domain.Dtos.Address;
using AcmeCorp.Domain.Dtos.Customer;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Domain.Models;
using Ardalis.GuardClauses;
using AutoMapper;

namespace AcmeCorp.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CustomerQueryDto> CreateAsync(CustomerCreateDto customer)
        {
            var createdCustomer = await _repository.CreateAsync(_mapper.Map(customer, new Customer()));
            return _mapper.Map<CustomerQueryDto>(createdCustomer);
        }

        public async Task<List<CustomerQueryDto>> GetAllAsync(bool includeArchived)
        {
            var customers = await _repository.GetAllAsync(includeArchived);
            return _mapper.Map<List<CustomerQueryDto>>(customers);
        }

        public async Task<CustomerQueryDto> GetByIdAsync(int id)
        {
            Guard.Against.NegativeOrZero(id);
            Guard.Against.Null(id);

            var customer = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerQueryDto>(customer);
        }

        public async Task UpdateAsync(CustomerUpdateDto customer)
        {
            Guard.Against.NegativeOrZero(customer.Id);
            Guard.Against.Null(customer.Id);

            var entity = await _repository.GetByIdAsync(customer.Id);

            Guard.Against.NotFound(customer.Id, entity);

            await _repository.UpdateAsync(_mapper.Map(customer, entity));
        }
    }
}
