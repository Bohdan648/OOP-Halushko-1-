using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        foreach (int size in new int[] { 1_000_000, 10_000_000 })
        {
            Random random = new Random(42);
            List<int> numbers = new List<int>(size);

            for (int i = 0; i < size; i++)
                numbers.Add(random.Next(1, 1_000_001));

            Stopwatch timer = Stopwatch.StartNew();

            long normal = numbers
                .Where(n => IsPrime(n))
                .Select(n => (long)n)
                .Sum();

            timer.Stop();
            double normalTime = timer.Elapsed.TotalMilliseconds;

            timer.Restart();

            long parallel = numbers
                .AsParallel()
                .Where(n => IsPrime(n))
                .Select(n => (long)n)
                .Sum();

            timer.Stop();

            Console.WriteLine($"\nЕлементів: {size}");
            Console.WriteLine($"LINQ: {normalTime:F2} мс");
            Console.WriteLine(
                $"PLINQ: {timer.Elapsed.TotalMilliseconds:F2} мс");

            Console.WriteLine($"Сума простих чисел: {normal}");
            Console.WriteLine($"Результати однакові: {normal == parallel}");
        }

        ShowRaceCondition();
    }

    static bool IsPrime(int number)
    {
        if (number < 2)
            return false;

        for (int divisor = 2; divisor * divisor <= number; divisor++)
        {
            if (number % divisor == 0)
                return false;
        }

        return true;
    }

    static void ShowRaceCondition()
    {
        Console.WriteLine("\nПеревірка спільного лічильника:");
        const int count = 1_000_000;

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            int wrong = 0;

            Enumerable.Range(1, count).AsParallel()
                .WithDegreeOfParallelism(4)
                .ForAll(n => wrong++);

            int correct = 0;

            Enumerable.Range(1, count).AsParallel()
                .WithDegreeOfParallelism(4)
                .ForAll(n => Interlocked.Increment(ref correct));

            Console.WriteLine($"Спроба {attempt}. Очікується: {count}");
            Console.WriteLine($"Без синхронізації: {wrong}");
            Console.WriteLine($"З Interlocked: {correct}");
        }
    }
}