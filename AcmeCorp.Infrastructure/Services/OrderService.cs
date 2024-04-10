using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Domain.Models;
using Ardalis.GuardClauses;
using AutoMapper;

namespace AcmeCorp.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<OrderQueryDto> CreateAsync(OrderCreateDto order)
        {
            var createdOrder = await _repository.CreateAsync(_mapper.Map(order, new Order()));
            return _mapper.Map<OrderQueryDto>(createdOrder);
        }

        public async Task<List<OrderQueryDto>> GetAllAsync(bool includeArchived)
        {
            var orders = await _repository.GetAllAsync(includeArchived);
            return _mapper.Map<List<OrderQueryDto>>(orders);
        }

        public async Task<List<OrderQueryDto>> GetByCustomerIdAsync(int customerId)
        {
            Guard.Against.NegativeOrZero(customerId);
            Guard.Against.Null(customerId);

            var orders = await _repository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<OrderQueryDto>>(orders);
        }

        public async Task<OrderQueryDto> GetByIdAsync(int id)
        {
            Guard.Against.NegativeOrZero(id);
            Guard.Against.Null(id);

            var order = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrderQueryDto>(order);
        }

        public async Task UpdateAsync(OrderUpdateDto order)
        {
            Guard.Against.NegativeOrZero(order.Id);
            Guard.Against.Null(order.Id);

            Guard.Against.NegativeOrZero(order.CustomerId);
            Guard.Against.Null(order.CustomerId);

            var entity = await _repository.GetByIdAsync(order.Id);

            Guard.Against.NotFound(order.Id, entity);

            await _repository.UpdateAsync(_mapper.Map(order, entity));
        }
    }
}
