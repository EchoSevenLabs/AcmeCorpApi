using AcmeCorp.Domain.Dtos.Address;
using AcmeCorp.Domain.Interfaces.Repositories;
using AcmeCorp.Domain.Models;
using AcmeCorp.Infrastructure.Services;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace AcmeCorp.UnitTests
{
    public class AddressServiceTests
    {
        private Mock<IAddressRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private AddressService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IAddressRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AddressService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CreateAsync_WithValidAddress_ReturnsAddressQueryDto()
        {
            var addressCreateDto = new AddressCreateDto();
            var address = new Address();
            var addressQueryDto = new AddressQueryDto();

            _mapperMock.Setup(m => m.Map<Address>(It.IsAny<AddressCreateDto>())).Returns(address);
            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Address>())).ReturnsAsync(address);
            _mapperMock.Setup(m => m.Map<AddressQueryDto>(It.IsAny<Address>())).Returns(addressQueryDto);

            var result = await _service.CreateAsync(addressCreateDto);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(addressQueryDto);
        }

        [Test]
        public async Task GetAllAsync_WithIncludeArchived_ReturnsListOfAddressQueryDtos()
        {
            var addresses = new List<Address> { new Address(), new Address() };
            var addressQueryDtos = new List<AddressQueryDto> { new AddressQueryDto(), new AddressQueryDto() };

            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<bool>())).ReturnsAsync(addresses);
            _mapperMock.Setup(m => m.Map<List<AddressQueryDto>>(It.IsAny<List<Address>>())).Returns(addressQueryDtos);

            var result = await _service.GetAllAsync(true);

            result.Should().NotBeNull();
            result.Should().HaveCount(addresses.Count);
            result.Should().BeEquivalentTo(addressQueryDtos);
        }

        [Test]
        public async Task GetByCustomerIdAsync_WithValidCustomerId_ReturnsListOfAddressQueryDtos()
        {
            int validCustomerId = 1;
            var addresses = new List<Address> { new Address(), new Address() };
            var addressQueryDtos = new List<AddressQueryDto> { new AddressQueryDto(), new AddressQueryDto() };

            _repositoryMock.Setup(r => r.GetByCustomerIdAsync(validCustomerId)).ReturnsAsync(addresses);
            _mapperMock.Setup(m => m.Map<List<AddressQueryDto>>(It.IsAny<List<Address>>())).Returns(addressQueryDtos);

            var result = await _service.GetByCustomerIdAsync(validCustomerId);

            result.Should().NotBeNull();
            result.Should().HaveCount(addresses.Count);
            result.Should().BeEquivalentTo(addressQueryDtos);
        }

        [Test]
        public void GetByCustomerIdAsync_WithInvalidCustomerId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetByCustomerIdAsync(0), "Exception for invalid customer id");
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsAddressQueryDto()
        {
            int validCustomerId = 1;
            var addresses = new Address();
            var addressQueryDto = new AddressQueryDto();

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(addresses);
            _mapperMock.Setup(m => m.Map<AddressQueryDto>(It.IsAny<Address>())).Returns(addressQueryDto);

            var result = await _service.GetByIdAsync(validCustomerId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(addressQueryDto);
        }

        [Test]
        public void GetByIdAsync_WithValidId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetByIdAsync(0), "Exception for invalid id");
        }

        [Test]
        public void UpdateAsync_WithInvalidId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateAsync(new AddressUpdateDto() { CustomerId = 1 }), "Exception for invalid id");
        }

        [Test]
        public void UpdateAsync_WithInvalidCustomerId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateAsync(new AddressUpdateDto() { Id = 1 }), "Exception for invalid customer id");
        }
    }
}
