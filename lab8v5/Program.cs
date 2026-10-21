using System;
using System.Collections.Generic;

namespace lab8v5
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Групування продуктів за категорією ===");
            var products = new List<Product>
            {
                new Product("Ноутбук", "Електроніка", 25000m),
                new Product("Яблуко", "Продукти", 30m),
                new Product("Смартфон", "Електроніка", 18000m),
                new Product("Хліб", "Продукти", 25m),
                new Product("Навушники", "Електроніка", 3000m)
            };

            var groupedProducts = CollectionHelper<Product>.GroupBy(products, p => p.Category);

            foreach (var group in groupedProducts)
            {
                Console.WriteLine($"\nКатегорія: {group.Key}");
                foreach (var prod in group.Value)
                {
                    Console.WriteLine($" - {prod.Name} ({prod.Price} UAH)");
                }
            }

            Console.WriteLine("\n==========================================");
            Console.WriteLine("=== 2. Групування слів за першою літерою ===");
            var words = new List<string> { "Яблуко", "Ананас", "Банан", "Апельсин", "Береза", "Ягода" };

            var groupedWords = CollectionHelper<string>.GroupBy(words, w => w[0]);

            foreach (var group in groupedWords)
            {
                Console.WriteLine($"\nЛітера '{group.Key}':");
                foreach (var word in group.Value)
                {
                    Console.WriteLine($" - {word}");
                }
            }
        }
    }
}