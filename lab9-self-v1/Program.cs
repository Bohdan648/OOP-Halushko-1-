using System;
using System.Collections.Generic;
using System.Text;

static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection, Func<TSource, TResult> mapper)
    {
        List<TResult> result = new List<TResult>();

        foreach (TSource item in collection)
            result.Add(mapper(item));

        return result;
    }

    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        using IEnumerator<T> iterator = collection.GetEnumerator();

        if (!iterator.MoveNext())
            throw new InvalidOperationException("Колекція порожня.");

        T maxItem = iterator.Current;
        TKey maxKey = keySelector(maxItem);

        while (iterator.MoveNext())
        {
            T item = iterator.Current;
            TKey key = keySelector(item);

            if (key.CompareTo(maxKey) > 0)
            {
                maxItem = item;
                maxKey = key;
            }
        }

        return maxItem;
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection, Predicate<T> predicate)
    {
        List<T> result = new List<T>();

        foreach (T item in collection)
        {
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public static void ForEach<T>(
        IEnumerable<T> collection, Action<T> action)
    {
        foreach (T item in collection)
            action(item);
    }
}

class Product
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<int> numbers = new List<int> { 3, 6, 2, 9, 4 };

        List<string> cities = new List<string>
        {
            "Київ", "Рівне", "Харків"
        };

        List<Product> products = new List<Product>
        {
            new Product { Name = "Навушники", Price = 1800 },
            new Product { Name = "Павербанк", Price = 2500 },
            new Product { Name = "Кабель", Price = 200 }
        };

        // Map: перетворення елементів
        var squares = CollectionUtils.Map(numbers, n => n * n);
        var names = CollectionUtils.Map(products, p => p.Name);

        Console.WriteLine("Квадрати: " + string.Join(", ", squares));
        Console.WriteLine("Назви товарів: " + string.Join(", ", names));

        // MaxBy: пошук елемента з найбільшим ключем
        string city = CollectionUtils.MaxBy(cities, c => c.Length);
        Product product = CollectionUtils.MaxBy(products, p => p.Price);

        Console.WriteLine("Найдовша назва міста: " + city);
        Console.WriteLine("Найдорожчий товар: " + product.Name +
            " - " + product.Price + " грн");

        // Filter: відбір за умовою
        var even = CollectionUtils.Filter(numbers, n => n % 2 == 0);
        var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);

        // ForEach: дія для кожного елемента
        Console.WriteLine("Парні числа:");
        CollectionUtils.ForEach(even, n => Console.WriteLine(n));

        Console.WriteLine("Товари дорожчі за 1000 грн:");
        CollectionUtils.ForEach(expensive,
            p => Console.WriteLine(p.Name + ": " + p.Price + " грн"));
    }
}