using AcmeCorp.Domain.Dtos.Customer;
using AcmeCorp.IntegrationTests.Factories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AcmeCorp.IntegrationTests
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private IConfiguration _configuration;
        private HttpClient _client;
        private AcmeCorpApiFactory<Program> _factory;

        [SetUp]
        public void SetUp()
        {
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

            var response = await _client.GetAsync("/api/customer/all");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        #endregion

        #region POST /api/customer

        [Test]
        public async Task CreateCustomer_WithRequiredFields_ReturnsOk()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerCreateDto
            {
                Email = "test@test.org",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task CreateCustomer_WithRequiredFields_ReturnsCustomer()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerCreateDto
            {
                Email = "test@test.org",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/customer", httpContent);
            var responseString = await result.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<CustomerQueryDto>(responseString);
            customer.Should().NotBeNull();
        }

        [Test]
        public async Task CreateCustomer_MissingEmail_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerCreateDto
            {
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateCustomer_InvalidEmail_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerCreateDto
            {
                Email = "test",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateCustomer_MissingFirstName_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerCreateDto
            {
                Email = "test@test.org",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateCustomer_MissingLastName_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerCreateDto
            {
                Email = "test@test.org",
                FirstName = "John",
                MiddleName = "Test",
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region GET /api/customer/all

        [Test]
        public async Task GetAllCustomers_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/customer/all");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAllCustomers_ReturnsListOfCustomers()
        {
            var response = await _client.GetAsync("/api/customer/all");

            var responseString = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<CustomerQueryDto>>(responseString);
            customers.Should().NotBeNull();
            customers.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region GET /api/customer/?id={id}

        [Test]
        public async Task GetCustomersById_WithId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/customer/?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetCustomersById_WithoutId_ReturnsInternalServerError()
        {
            var response = await _client.GetAsync("/api/customer");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetCustomersById_ReturnsCustomer()
        {
            var response = await _client.GetAsync("/api/customer/?id=1");

            var responseString = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<CustomerQueryDto>(responseString);
            customers.Should().NotBeNull();
        }

        #endregion

        #region PUT /api/customer

        [Test]
        public async Task UpdateCustomer_WithRequiredFields_ReturnsOk()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerUpdateDto
            {
                Id = 1,
                Email = "test@test.org",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task UpdateCustomer_MissingId_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerUpdateDto
            {
                Email = "test@test.org",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateCustomer_MissingEmail_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerUpdateDto
            {
                Id = 1,
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateCustomer_InvalidEmail_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerUpdateDto
            {
                Id = 1,
                Email = "test",
                FirstName = "John",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateCustomer_MissingFirstName_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerUpdateDto
            {
                Id = 1,
                Email = "test@test.org",
                MiddleName = "Test",
                LastName = "Doe"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateCustomer_MissingLastName_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new CustomerUpdateDto
            {
                Id = 1,
                Email = "test@test.org",
                FirstName = "John",
                MiddleName = "Test"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/customer", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}
