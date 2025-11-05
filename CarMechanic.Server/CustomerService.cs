using CarMechanic.Server.Data;
using CarMechanic.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CarMechanic.Server
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer != null)
            {
                var jobsToDelete = await _context.Jobs.Where(j => j.CustomerId == id).ToListAsync();

                if (jobsToDelete.Any())
                { 
                    _context.Jobs.RemoveRange(jobsToDelete);
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (existingCustomer != null)
            {
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Customer not found");
            }
        }
    }
}