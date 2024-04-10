using AcmeCorp.Domain.Dtos.Address;
using AcmeCorp.Domain.Interfaces.Services;
using AcmeCorp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.API.Controllers
{
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressCreateDto address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAddress = await _service.CreateAsync(address);
            return Ok(createdAddress);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAddresses(bool includeArchived = true)
        {
            var addresses = await _service.GetAllAsync(includeArchived);
            if (addresses == null || !addresses.Any())
            {
                return NotFound();
            }

            return Ok(addresses);
        }

        [HttpGet("bycustomer")]
        public async Task<IActionResult> GetAddressesByCustomerId(int customerId)
        {
            var address = await _service.GetByCustomerIdAsync(customerId);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddress(AddressUpdateDto address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(address);
            return Ok();
        }
    }
}
