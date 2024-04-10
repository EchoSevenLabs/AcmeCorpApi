using AcmeCorp.Domain.Dtos.Address;
using AcmeCorp.Domain.Enums;
using AcmeCorp.Domain.Models;
using AcmeCorp.IntegrationTests.Factories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AcmeCorp.IntegrationTests
{
    [TestFixture]
    public class AddressControllerTests
    {
        private IConfiguration _configuration;
        private HttpClient _client;
        private AcmeCorpApiFactory<Program> _factory;

        [SetUp]
        public void SetUp()
        {
            // Set up IConfiguration instance
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.development.json", optional: true)
                .Build();

            _factory = new AcmeCorpApiFactory<Program>();
            _client = _factory.CreateClient();

            var apiKey = _configuration["ApiConfig:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            }
        }

        [TearDown]
        public void Cleanup()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        #region Controller Authorization

        [Test]
        public async Task RequestWithoutApiKey_ReturnsUnauthorized()
        {
            _client.DefaultRequestHeaders.Remove("X-Api-Key");

            var response = await _client.GetAsync("/api/address/all");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        #endregion

        #region POST /api/address

        [Test]
        public async Task CreateAddress_WithRequiredFields_ReturnsOk()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Portland",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task CreateAddress_WithRequiredFields_ReturnsAddress()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "12345 Mockingbird Lane",
                City = "Portland",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            var responseString = await result.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<AddressQueryDto>(responseString);
            addresses.Should().NotBeNull();
        }

        [Test]
        public async Task CreateAddress_MissingCustomerId_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateAddress_CustomerIdEqualsZero_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 0,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateAddress_MissingStreet1_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Work,
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateAddress_MissingCity_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateAddress_MissingStateProvince_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateAddress_MissingPostalCode_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateAddress_MissingCountry_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressCreateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region GET /api/address/all

        [Test]
        public async Task GetAllAddresses_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/address/all");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAllAddresses_ReturnsListOfAddresses()
        {
            var response = await _client.GetAsync("/api/address/all");

            var responseString = await response.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<List<AddressQueryDto>>(responseString);
            addresses.Should().NotBeNull();
            addresses.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region GET /api/address/bycustomer/?customerid={customerid}

        [Test]
        public async Task GetAddressesByCustomer_WithCustomerId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/address/bycustomer/?customerid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAddressesByCustomer_WithoutCustomerId_ReturnsInternalServerError()
        {
            var response = await _client.GetAsync("/api/address/bycustomer");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetAddressesByCustomer_ReturnsListOfAddresses()
        {
            var response = await _client.GetAsync("/api/address/bycustomer/?customerid=1");

            var responseString = await response.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<List<AddressQueryDto>>(responseString);
            addresses.Should().NotBeNull();
            addresses.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region GET /api/address/?id={id}

        [Test]
        public async Task GetAddressesById_WithId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/address/?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAddressesById_WithoutId_ReturnsInternalServerError()
        {
            var response = await _client.GetAsync("/api/address");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetAddressesById_ReturnsAddress()
        {
            var response = await _client.GetAsync("/api/address/?id=1");

            var responseString = await response.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<AddressQueryDto>(responseString);
            addresses.Should().NotBeNull();
        }

        #endregion

        #region PUT /api/address

        [Test]
        public async Task UpdateAddress_WithRequiredFields_ReturnsOk()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Portland",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task UpdateAddress_MissingId_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateAddress_MissingStreet1_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Type = AddressType.Home,
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateAddress_MissingCity_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                StateProvince = "OR",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateAddress_MissingStateProvince_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                PostalCode = "97211",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateAddress_MissingPostalCode_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                Country = "US"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateAddress_MissingCountry_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new AddressUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Type = AddressType.Home,
                Street1 = "1234 Mockingbird Lane",
                City = "Porltand",
                StateProvince = "OR",
                PostalCode = "97211",
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/address", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}
