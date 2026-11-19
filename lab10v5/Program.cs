using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace lab10v5
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Олександр", City = "Kyiv" },
                new Customer { Id = 2, Name = "Марія", City = "Lviv" },
                new Customer { Id = 3, Name = "Іван", City = "Kyiv" },
                new Customer { Id = 4, Name = "Олена", City = "Odesa" },
                new Customer { Id = 5, Name = "Дмитро", City = "Kyiv" },
                new Customer { Id = 6, Name = "Анна", City = "Kharkiv" },
                new Customer { Id = 7, Name = "Максим", City = "Lviv" },
                new Customer { Id = 8, Name = "Софія", City = "Kyiv" },
                new Customer { Id = 9, Name = "Артем", City = "Dnipro" },
                new Customer { Id = 10, Name = "Вікторія", City = "Kyiv" }
            };

            var orders = new List<Order>
            {
                new Order { Id = 101, OrderDate = new DateTime(2026, 10, 1), CustomerId = 1, TotalAmount = 1500m },
                new Order { Id = 102, OrderDate = new DateTime(2026, 10, 2), CustomerId = 2, TotalAmount = 2200m },
                new Order { Id = 103, OrderDate = new DateTime(2026, 10, 3), CustomerId = 3, TotalAmount = 800m },
                new Order { Id = 104, OrderDate = new DateTime(2026, 10, 4), CustomerId = 1, TotalAmount = 3200m },
                new Order { Id = 105, OrderDate = new DateTime(2026, 10, 5), CustomerId = 4, TotalAmount = 1100m },
                new Order { Id = 106, OrderDate = new DateTime(2026, 10, 6), CustomerId = 5, TotalAmount = 4500m },
                new Order { Id = 107, OrderDate = new DateTime(2026, 10, 7), CustomerId = 6, TotalAmount = 950m },
                new Order { Id = 108, OrderDate = new DateTime(2026, 10, 8), CustomerId = 8, TotalAmount = 1250m },
                new Order { Id = 109, OrderDate = new DateTime(2026, 10, 9), CustomerId = 7, TotalAmount = 1900m },
                new Order { Id = 110, OrderDate = new DateTime(2026, 10, 10), CustomerId = 10, TotalAmount = 600m },
                new Order { Id = 111, OrderDate = new DateTime(2026, 10, 11), CustomerId = 3, TotalAmount = 2750m },
                new Order { Id = 112, OrderDate = new DateTime(2026, 10, 12), CustomerId = 9, TotalAmount = 1400m }
            };

            var sw = new Stopwatch();

            // 1. Замовлення з Kyiv на суму > 1000
            Console.WriteLine("=== 1. Замовлення з Kyiv (> 1000) ===");
            sw.Restart();
            var q1M = orders.Join(customers, o => o.CustomerId, c => c.Id, (o, c) => new { o, c })
                .Where(x => x.c.City == "Kyiv" && x.o.TotalAmount > 1000m)
                .Select(x => new { x.o.Id, x.c.Name, x.o.TotalAmount }).ToList();
            sw.Stop();
            long t1M = sw.ElapsedTicks;

            sw.Restart();
            var q1Q = (from o in orders
                       join c in customers on o.CustomerId equals c.Id
                       where c.City == "Kyiv" && o.TotalAmount > 1000m
                       select new { o.Id, c.Name, o.TotalAmount }).ToList();
            sw.Stop();
            long t1Q = sw.ElapsedTicks;

            foreach (var i in q1M)
                Console.WriteLine($"#{i.Id} | {i.Name} | {i.TotalAmount} грн");
            Console.WriteLine($"Час: Method = {t1M} ticks, Query = {t1Q} ticks\n");

            // 2. Об'єднання замовлень з клієнтами
            Console.WriteLine("=== 2. Дата замовлення та ім'я клієнта ===");
            sw.Restart();
            var q2M = orders.Join(customers, o => o.CustomerId, c => c.Id, (o, c) => new { o.OrderDate, c.Name }).ToList();
            sw.Stop();
            long t2M = sw.ElapsedTicks;

            sw.Restart();
            var q2Q = (from o in orders
                       join c in customers on o.CustomerId equals c.Id
                       select new { o.OrderDate, c.Name }).ToList();
            sw.Stop();
            long t2Q = sw.ElapsedTicks;

            foreach (var i in q2M.Take(5))
                Console.WriteLine($"{i.OrderDate:yyyy-MM-dd} | {i.Name}");
            Console.WriteLine($"Час: Method = {t2M} ticks, Query = {t2Q} ticks\n");

            // 3. Загальна сума замовлень для кожного клієнта
            Console.WriteLine("=== 3. Сума замовлень по клієнтах ===");
            sw.Restart();
            var q3M = customers.GroupJoin(orders, c => c.Id, o => o.CustomerId,
                (c, co) => new { c.Name, Total = co.Sum(o => o.TotalAmount) }).ToList();
            sw.Stop();
            long t3M = sw.ElapsedTicks;

            sw.Restart();
            var q3Q = (from c in customers
                       join o in orders on c.Id equals o.CustomerId into co
                       select new { c.Name, Total = co.Sum(o => o.TotalAmount) }).ToList();
            sw.Stop();
            long t3Q = sw.ElapsedTicks;

            foreach (var i in q3M)
                Console.WriteLine($"{i.Name}: {i.Total} грн");
            Console.WriteLine($"Час: Method = {t3M} ticks, Query = {t3Q} ticks\n");

            // 4. Замовлення з максимальною сумою
            Console.WriteLine("=== 4. Максимальне замовлення ===");
            sw.Restart();
            var q4M = orders.OrderByDescending(o => o.TotalAmount).FirstOrDefault();
            sw.Stop();
            long t4M = sw.ElapsedTicks;

            sw.Restart();
            var q4Q = (from o in orders orderby o.TotalAmount descending select o).FirstOrDefault();
            sw.Stop();
            long t4Q = sw.ElapsedTicks;

            Console.WriteLine($"#{q4M?.Id} | Сума: {q4M?.TotalAmount} грн");
            Console.WriteLine($"Час: Method = {t4M} ticks, Query = {t4Q} ticks");
        }
    }
}