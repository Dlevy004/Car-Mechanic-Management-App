using Microsoft.AspNetCore.Mvc;
using CarMechanic.Shared.Models;

namespace CarMechanic.Server.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound(); // 404-es hibát dob, ha nincs ilyen ügyfél
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<Customer> PostAsync([FromBody] Customer customer)
        {
            var createdCustomer = await _customerService.AddCustomerAsync(customer);
            // Visszaadjuk a létrehozott ügyfelet és egy 201 Created státuszkódot
            return createdCustomer;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest(); // 400-as hiba, ha az ID-k nem egyeznek
            }

            await _customerService.UpdateCustomerAsync(customer);
            return NoContent(); // 204-es státuszkód, sikeres frissítés esetén
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return NoContent(); // 204-es státuszkód, sikeres törlés esetén
        }
    }
}
