using AcmeCorp.Domain.Dtos.Customer;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Services;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace AcmeCorp.UnitTests
{
    internal class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private CustomerService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new CustomerService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateAsync_WithValidCustomer_ReturnsCustomerQueryDto()
        {
            var customerCreateDto = new CustomerCreateDto();
            var customer = new Customer();
            var customerQueryDto = new CustomerQueryDto();

            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CustomerCreateDto>())).Returns(customer);
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Customer>())).ReturnsAsync(customer);
            _mapperMock.Setup(m => m.Map<CustomerQueryDto>(It.IsAny<Customer>())).Returns(customerQueryDto);

            var result = await _service.CreateAsync(customerCreateDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(customerQueryDto);
        }

        [Test]
        public async Task GetAllAsync_WithIncludeArchived_ReturnsListOfCustomerQueryDtos()
        {
            var customers = new List<Customer> { new Customer(), new Customer() };
            var customerQueryDtos = new List<CustomerQueryDto> { new CustomerQueryDto(), new CustomerQueryDto() };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ReturnsAsync(customers);
            _mapperMock.Setup(m => m.Map<List<CustomerQueryDto>>(It.IsAny<List<Customer>>())).Returns(customerQueryDtos);

            var result = await _service.GetAllAsync(true);

            result.Should().NotBeNull();
            result.Should().HaveCount(customers.Count);
            result.Should().BeEquivalentTo(customerQueryDtos);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsCustomerQueryDto()
        {
            int validCustomerId = 1;
            var customers = new Customer();
            var customerQueryDto = new CustomerQueryDto();

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customers);
            _mapperMock.Setup(m => m.Map<CustomerQueryDto>(It.IsAny<Customer>())).Returns(customerQueryDto);

            var result = await _service.GetByIdAsync(validCustomerId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(customerQueryDto);
        }

        [Test]
        public void GetByIdAsync_WithValidId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetByIdAsync(0), "Exception for invalid id");
        }

        [Test]
        public void UpdateAsync_WithInvalidCustomerId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateAsync(new CustomerUpdateDto()), "Exception for invalid id");
        }
    }
}
