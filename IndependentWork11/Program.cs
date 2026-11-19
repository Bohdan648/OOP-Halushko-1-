using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

class Sale
{
    public int Id, ProductId, Quantity, CustomerId;
    public DateTime SaleDate;
}

class Product
{
    public int Id;
    public string Name = "", Category = "";
    public decimal UnitPrice;
}

class Customer
{
    public int Id;
    public string Name = "", City = "";
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var sales = new List<Sale>();
        var products = new List<Product>();
        var customers = new List<Customer>();

        Random random = new Random(42);
        string[] categories = { "Електроніка", "Одяг", "Книги" };
        string[] cities = { "Київ", "Рівне", "Львів", "Одеса", "Харків" };

        for (int i = 1; i <= 100; i++)
        {
            products.Add(new Product
            {
                Id = i,
                Name = "Товар " + i,
                Category = categories[random.Next(3)],
                UnitPrice = random.Next(100, 5001)
            });

            customers.Add(new Customer
            {
                Id = i,
                Name = "Клієнт " + i,
                City = cities[random.Next(5)]
            });

            sales.Add(new Sale
            {
                Id = i,
                ProductId = random.Next(1, 101),
                CustomerId = random.Next(1, 101),
                Quantity = random.Next(1, 6),
                SaleDate = new DateTime(2025, 1, 1)
                    .AddDays(random.Next(730))
            });
        }

        TopProducts(sales, products);
        RevenueByCity(sales, products, customers);
        SalesByMonth(sales, products);
    }

    static void TopProducts(List<Sale> sales, List<Product> products)
    {
        Stopwatch timer = Stopwatch.StartNew();

        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { Id = g.Key, Total = g.Sum(s => s.Quantity) })
            .Join(products, s => s.Id, p => p.Id,
                (s, p) => new { p.Id, p.Name, p.Category, s.Total })
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                Top = g.OrderByDescending(p => p.Total)
                    .ThenBy(p => p.Id).Take(3).ToList()
            }).ToList();

        timer.Stop();

        Console.WriteLine("\n1. Найпопулярніші товари");

        foreach (var group in result)
        {
            Console.WriteLine(group.Category + ":");

            foreach (var p in group.Top)
                Console.WriteLine($"{p.Name}: {p.Total} шт.");
        }

        Console.WriteLine($"Час: {timer.Elapsed.TotalMilliseconds:F4} мс");
    }

    static void RevenueByCity(List<Sale> sales,
        List<Product> products, List<Customer> customers)
    {
        Stopwatch timer = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                (s, p) => new
                {
                    s.CustomerId,
                    Revenue = s.Quantity * p.UnitPrice
                })
            .Join(customers, s => s.CustomerId, c => c.Id,
                (s, c) => new { c.City, s.Revenue })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, Total = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.Total)
            .ToList();

        timer.Stop();

        Console.WriteLine("\n2. Дохід за містами");

        foreach (var city in result)
            Console.WriteLine($"{city.City}: {city.Total:F2} грн");

        Console.WriteLine($"Час: {timer.Elapsed.TotalMilliseconds:F4} мс");
    }

    static void SalesByMonth(List<Sale> sales, List<Product> products)
    {
        Stopwatch timer = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id,
                (s, p) => new
                {
                    s.SaleDate,
                    s.Quantity,
                    Revenue = s.Quantity * p.UnitPrice
                })
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Quantity = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        timer.Stop();

        Console.WriteLine("\n3. Продажі за місяцями");

        foreach (var month in result)
            Console.WriteLine($"{month.Year}-{month.Month:D2}: " +
                $"{month.Quantity} шт., {month.Revenue:F2} грн");

        Console.WriteLine($"Час: {timer.Elapsed.TotalMilliseconds:F4} мс");
    }
}