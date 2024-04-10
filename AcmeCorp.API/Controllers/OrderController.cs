using AcmeCorp.Domain.Dtos.Order;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.API.Controllers
{
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdOrder = await _service.CreateAsync(order);
            return Ok(createdOrder);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders(bool includeArchived = true)
        {
            var orders = await _service.GetAllAsync(includeArchived);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpGet("bycustomer")]
        public async Task<IActionResult> GetOrdersByCustomerId(int customerId)
        {
            var order = await _service.GetByCustomerIdAsync(customerId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(OrderUpdateDto order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(order);
            return Ok();
        }
    }
}
