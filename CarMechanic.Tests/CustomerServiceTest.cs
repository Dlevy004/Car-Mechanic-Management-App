using CarMechanic.Shared.Models;
using CarMechanic.Server.Data;
using Microsoft.EntityFrameworkCore;
using CarMechanic.Server;

namespace CarMechanic.Tests
{
    public class CustomerServiceTest
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test Elek", Email = "test@elek.hu", Address = "Debrecen" },
                new Customer { Id = 2, Name = "Minta János", Email = "minta@janos.hu", Address = "Budapest" }
            };
            dbContext.Customers.AddRange(customers);
            await dbContext.SaveChangesAsync();

            var service = new CustomerService(dbContext);

            // Act
            var result = await service.GetAllCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Elek", result[0].Name);
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldAddCustomerAndReturnIt()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new CustomerService(dbContext);
            var newCustomer = new Customer { Name = "Új Ügyfél", Email = "uj@ugyfel.com", Address = "Szeged" };

            // Act
            var createdCustomer = await service.AddCustomerAsync(newCustomer);
            var customerFromDb = await dbContext.Customers.FirstOrDefaultAsync(c => c.Name == "Új Ügyfél");

            // Assert
            Assert.NotNull(createdCustomer);
            Assert.Equal("Új Ügyfél", createdCustomer.Name);
            Assert.True(createdCustomer.Id > 0);

            Assert.NotNull(customerFromDb);
            Assert.Equal("uj@ugyfel.com", customerFromDb.Email);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCorrectCustomer_IfExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var customer = new Customer { Id = 5, Name = "Test Elek", Email = "test@elek.hu", Address = "Debrecen" };
            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();
            var service = new CustomerService(dbContext);

            // Act
            var result = await service.GetCustomerByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("Test Elek", result.Name);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnNull_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new CustomerService(dbContext);

            // Act
            var result = await service.GetCustomerByIdAsync(99); // Nem létező ID

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomer_IfExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var originalCustomer = new Customer { Id = 1, Name = "Régi Név", Email = "regi@email.com", Address = "Régi Cím" };
            dbContext.Customers.Add(originalCustomer);
            await dbContext.SaveChangesAsync();

            var service = new CustomerService(dbContext);
            var updatedCustomerData = new Customer { Id = 1, Name = "Új Név", Email = "uj@email.com", Address = "Új Cím" };

            // Act
            await service.UpdateCustomerAsync(updatedCustomerData);
            var customerFromDb = await dbContext.Customers.FindAsync(1);

            // Assert
            Assert.NotNull(customerFromDb);
            Assert.Equal("Új Név", customerFromDb.Name);
            Assert.Equal("uj@email.com", customerFromDb.Email);
            Assert.Equal("Új Cím", customerFromDb.Address);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldThrowKeyNotFoundException_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext(); // Üres DB
            var service = new CustomerService(dbContext);
            var nonExistentCustomer = new Customer { Id = 99, Name = "Nem létező", Email = "nincs@ilyen.com", Address = "Sehol" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateCustomerAsync(nonExistentCustomer));
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldRemoveCustomer_IfExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var customerToDelete = new Customer { Id = 1, Name = "Törlendő Elek", Email = "torlendo@elek.hu", Address = "Valahol" };
            dbContext.Customers.Add(customerToDelete);
            await dbContext.SaveChangesAsync();

            var service = new CustomerService(dbContext);

            // Act
            await service.DeleteCustomerAsync(1);
            var customerFromDb = await dbContext.Customers.FindAsync(1);
            var remainingCount = await dbContext.Customers.CountAsync();

            // Assert
            Assert.Null(customerFromDb);
            Assert.Equal(0, remainingCount);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldDoNothing_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var existingCustomer = new Customer { Id = 1, Name = "Maradó Elek", Email = "marado@elek.hu", Address = "Itt" };
            dbContext.Customers.Add(existingCustomer);
            await dbContext.SaveChangesAsync();

            var service = new CustomerService(dbContext);

            // Act
            await service.DeleteCustomerAsync(99);
            var customerFromDb = await dbContext.Customers.FindAsync(1);
            var remainingCount = await dbContext.Customers.CountAsync();


            // Assert
            Assert.NotNull(customerFromDb);
            Assert.Equal(1, remainingCount);
        }
    }
}
