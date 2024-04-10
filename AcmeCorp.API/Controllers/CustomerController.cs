using AcmeCorp.Domain.Dtos.Customer;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.API.Controllers
{
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Domain.Dtos.Customer.CustomerCreateDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCustomer = await _service.CreateAsync(customer);
            return Ok(createdCustomer);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCustomers(bool includeArchived = true)
        {
            var customers = await _service.GetAllAsync(includeArchived);
            if (customers == null || !customers.Any())
            {
                return NotFound();
            }

            return Ok(customers);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(customer);
            return Ok();
        }
    }
}
