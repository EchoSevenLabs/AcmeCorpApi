using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Services;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace AcmeCorp.UnitTests
{
    internal class OrderServiceTests
    {
        private Mock<IOrderRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private OrderService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new OrderService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateAsync_WithValidOrder_ReturnsOrderQueryDto()
        {
            var orderCreateDto = new OrderCreateDto();
            var order = new Order();
            var orderQueryDto = new OrderQueryDto();

            _mapperMock.Setup(m => m.Map<Order>(It.IsAny<OrderCreateDto>())).Returns(order);
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _mapperMock.Setup(m => m.Map<OrderQueryDto>(It.IsAny<Order>())).Returns(orderQueryDto);

            var result = await _service.CreateAsync(orderCreateDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(orderQueryDto);
        }

        [Test]
        public async Task GetAllAsync_WithIncludeArchived_ReturnsListOfOrderQueryDtos()
        {
            var orders = new List<Order> { new Order(), new Order() };
            var orderQueryDtos = new List<OrderQueryDto> { new OrderQueryDto(), new OrderQueryDto() };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ReturnsAsync(orders);
            _mapperMock.Setup(m => m.Map<List<OrderQueryDto>>(It.IsAny<List<Order>>())).Returns(orderQueryDtos);

            var result = await _service.GetAllAsync(true);

            result.Should().NotBeNull();
            result.Should().HaveCount(orders.Count);
            result.Should().BeEquivalentTo(orderQueryDtos);
        }

        [Test]
        public async Task GetByCustomerIdAsync_WithValidCustomerId_ReturnsListOfOrderQueryDtos()
        {
            int validCustomerId = 1;
            var orders = new List<Order> { new Order(), new Order() };
            var orderQueryDtos = new List<OrderQueryDto> { new OrderQueryDto(), new OrderQueryDto() };

            _repositoryMock.Setup(r => r.GetByCustomerIdAsync(validCustomerId)).ReturnsAsync(orders);
            _mapperMock.Setup(m => m.Map<List<OrderQueryDto>>(It.IsAny<List<Order>>())).Returns(orderQueryDtos);

            var result = await _service.GetByCustomerIdAsync(validCustomerId);

            result.Should().NotBeNull();
            result.Should().HaveCount(orders.Count);
            result.Should().BeEquivalentTo(orderQueryDtos);
        }

        [Test]
        public void GetByCustomerIdAsync_WithInvalidCustomerId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetByCustomerIdAsync(0), "Exception for invalid customer id");
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsOrderQueryDto()
        {
            int validCustomerId = 1;
            var orders = new Order();
            var orderQueryDto = new OrderQueryDto();

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(orders);
            _mapperMock.Setup(m => m.Map<OrderQueryDto>(It.IsAny<Order>())).Returns(orderQueryDto);

            var result = await _service.GetByIdAsync(validCustomerId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(orderQueryDto);
        }

        [Test]
        public void GetByIdAsync_WithValidId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetByIdAsync(0), "Exception for invalid id");
        }

        [Test]
        public void UpdateAsync_WithInvalidId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateAsync(new OrderUpdateDto() { CustomerId = 1 }), "Exception for invalid id");
        }

        [Test]
        public void UpdateAsync_WithInvalidCustomerId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateAsync(new OrderUpdateDto() { Id = 1 }), "Exception for invalid customer id");
        }
    }
}
