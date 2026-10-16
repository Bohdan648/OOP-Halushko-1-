using System;

namespace lab7v5
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IRepository<Customer> repository = new InMemoryRepository<Customer>();

            repository.Add(new Customer { Id = 1, FirstName = "Богдан", LastName = "Галушко", Address = "Київ" });
            repository.Add(new Customer { Id = 2, FirstName = "Іван", LastName = "Петренко", Address = "Львів" });

            Console.WriteLine("=== Список усіх клієнтів ===");
            foreach (var cust in repository.GetAll())
            {
                Console.WriteLine($"ID: {cust.Id} | {cust.FirstName} {cust.LastName} | {cust.Address}");
            }

            Console.WriteLine("\n=== Оновлення клієнта ID = 1 ===");
            repository.Update(new Customer { Id = 1, FirstName = "Богдан", LastName = "Галушко", Address = "Рівне" });
            var updated = repository.GetById(1);
            Console.WriteLine($"Оновлена адреса ID 1: {updated?.Address}");

            Console.WriteLine("\n=== Видалення клієнта ID = 2 ===");
            repository.Delete(2);
            Console.WriteLine($"Кількість після видалення: {((InMemoryRepository<Customer>)repository).GetAll().Count()}");
        }
    }
}