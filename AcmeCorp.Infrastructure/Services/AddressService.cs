using AcmeCorp.Domain.Dtos.Address;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Domain.Models;
using Ardalis.GuardClauses;
using AutoMapper;

namespace AcmeCorp.Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly IAddressRepository _repository;

        public AddressService(IAddressRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<AddressQueryDto> CreateAsync(AddressCreateDto address)
        {
            var createdAddress = await _repository.CreateAsync(_mapper.Map(address, new Address()));
            return _mapper.Map<AddressQueryDto>(createdAddress);
        }

        public async Task<List<AddressQueryDto>> GetAllAsync(bool includeArchived)
        {
            var addresses = await _repository.GetAllAsync(includeArchived);
            return _mapper.Map<List<AddressQueryDto>>(addresses);
        }

        public async Task<List<AddressQueryDto>> GetByCustomerIdAsync(int customerId)
        {
            Guard.Against.NegativeOrZero(customerId);
            Guard.Against.Null(customerId);

            var addresses = await _repository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<AddressQueryDto>>(addresses);
        }

        public async Task<AddressQueryDto> GetByIdAsync(int id)
        {
            Guard.Against.NegativeOrZero(id);
            Guard.Against.Null(id);

            var address = await _repository.GetByIdAsync(id);
            return _mapper.Map<AddressQueryDto>(address);
        }

        public async Task UpdateAsync(AddressUpdateDto address)
        {
            Guard.Against.NegativeOrZero(address.Id);
            Guard.Against.Null(address.Id);

            Guard.Against.NegativeOrZero(address.CustomerId);
            Guard.Against.Null(address.CustomerId);

            var entity = await _repository.GetByIdAsync(address.Id);

            Guard.Against.NotFound(address.Id, entity);

            await _repository.UpdateAsync(_mapper.Map(address, entity));
        }
    }
}
