using AcmeCorp.Domain.Dtos.Order;
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
    public class OrderControllerTests
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

            var response = await _client.GetAsync("/api/order/all");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        #endregion

        #region POST /api/order

        [Test]
        public async Task CreateOrder_WithRequiredFields_ReturnsOk()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderCreateDto
            {
                CustomerId = 1,
                Status = OrderStatus.InProgress,
                ShipMethod = "UPS"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task CreateOrder_WithRequiredFields_ReturnsOrder()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderCreateDto
            {
                CustomerId = 1,
                Status = OrderStatus.InProgress,
                ShipMethod = "UPS"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/order", httpContent);
            var responseString = await result.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderQueryDto>(responseString);
            order.Should().NotBeNull();
        }

        [Test]
        public async Task CreateOrder_MissingCustomerId_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderCreateDto
            {
                Status = OrderStatus.InProgress,
                ShipMethod = "UPS"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateOrder_MissingShipMethod_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderCreateDto
            {
                CustomerId = 1,
                Status = OrderStatus.InProgress
            }), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region GET /api/order/all

        [Test]
        public async Task GetAllOrders_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/order/all");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAllOrders_ReturnsListOfOrders()
        {
            var response = await _client.GetAsync("/api/order/all");

            var responseString = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<OrderQueryDto>>(responseString);
            orders.Should().NotBeNull();
            orders.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region GET /api/order/bycustomer/?customerid={customerid}

        [Test]
        public async Task GetOrdersByCustomer_WithCustomerId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/order/bycustomer/?customerid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetOrdersByCustomer_WithoutCustomerId_ReturnsInternalServerError()
        {
            var response = await _client.GetAsync("/api/order/bycustomer");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetOrdersByCustomer_ReturnsListOfOrders()
        {
            var response = await _client.GetAsync("/api/order/bycustomer/?customerid=1");

            var responseString = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<OrderQueryDto>>(responseString);
            orders.Should().NotBeNull();
            orders.Should().HaveCountGreaterThan(0);
        }

        #endregion

        #region GET /api/order/?id={id}

        [Test]
        public async Task GetOrdersById_WithId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/order/?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetOrdersById_WithoutId_ReturnsInternalServerError()
        {
            var response = await _client.GetAsync("/api/order");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetOrdersById_ReturnsOrder()
        {
            var response = await _client.GetAsync("/api/order/?id=1");

            var responseString = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<OrderQueryDto>(responseString);
            orders.Should().NotBeNull();
        }

        #endregion

        #region PUT /api/order

        [Test]
        public async Task UpdateOrder_WithRequiredFields_ReturnsOk()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Status = OrderStatus.InProgress,
                ShipMethod = "UPS"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task UpdateOrder_MissingId_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderUpdateDto
            {
                CustomerId = 1,
                Status = OrderStatus.InProgress,
                ShipMethod = "UPS"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateOrder_MissingCustomerId_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderUpdateDto
            {
                Id = 1,
                Status = OrderStatus.InProgress,
                ShipMethod = "UPS"
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateOrder_MissingShipMethod_ReturnsBadRequest()
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(new OrderUpdateDto
            {
                Id = 1,
                CustomerId = 1,
                Status = OrderStatus.InProgress
            }), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/api/order", httpContent);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}
