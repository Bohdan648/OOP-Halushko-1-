using System;

using System.Collections.Generic;

using System.Linq;

using lab7v5;

using Xunit;

namespace lab7v5.Tests
{
    public class InMemoryRepositoryTests
    {
        private InMemoryRepository<Customer> CreateRepository()
        {
            return new InMemoryRepository<Customer>();
        }

        [Fact]
        public void Add_ShouldAddCustomer()
        {
            var repo = CreateRepository();
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Address = "Street 1" };

            repo.Add(customer);

            var result = repo.GetById(1);
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public void Add_DuplicateId_ShouldThrowException()
        {
            var repo = CreateRepository();
            var customer1 = new Customer { Id = 1, FirstName = "John", LastName = "Doe" };
            var customer2 = new Customer { Id = 1, FirstName = "Jane", LastName = "Doe" };

            repo.Add(customer1);

            Assert.Throws<InvalidOperationException>(() => repo.Add(customer2));
        }

        [Fact]
        public void GetById_ExistingId_ShouldReturnCustomer()
        {
            var repo = CreateRepository();
            repo.Add(new Customer { Id = 10, FirstName = "Alex", LastName = "Smith" });

            var result = repo.GetById(10);

            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
        }

        [Fact]
        public void GetById_NonExistingId_ShouldReturnNull()
        {
            var repo = CreateRepository();

            var result = repo.GetById(99);

            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllCustomers()
        {
            var repo = CreateRepository();
            repo.Add(new Customer { Id = 1, FirstName = "A", LastName = "B" });
            repo.Add(new Customer { Id = 2, FirstName = "C", LastName = "D" });

            var all = repo.GetAll().ToList();

            Assert.Equal(2, all.Count);
        }

        [Fact]
        public void Update_ExistingCustomer_ShouldUpdateData()
        {
            var repo = CreateRepository();
            repo.Add(new Customer { Id = 1, FirstName = "Old", LastName = "Name", Address = "Old Addr" });

            repo.Update(new Customer { Id = 1, FirstName = "New", LastName = "Name", Address = "New Addr" });

            var updated = repo.GetById(1);
            Assert.NotNull(updated);
            Assert.Equal("New", updated.FirstName);
            Assert.Equal("New Addr", updated.Address);
        }

        [Fact]
        public void Update_NonExistingCustomer_ShouldThrowException()
        {
            var repo = CreateRepository();

            Assert.Throws<KeyNotFoundException>(() => 
                repo.Update(new Customer { Id = 5, FirstName = "Test", LastName = "Test" }));
        }

        [Fact]
        public void Delete_ExistingCustomer_ShouldRemove()
        {
            var repo = CreateRepository();
            repo.Add(new Customer { Id = 1, FirstName = "Tom", LastName = "Hardy" });

            repo.Delete(1);

            Assert.Null(repo.GetById(1));
        }

        [Fact]
        public void Delete_NonExistingCustomer_ShouldThrowException()
        {
            var repo = CreateRepository();

            Assert.Throws<KeyNotFoundException>(() => repo.Delete(999));
        }
    }
}