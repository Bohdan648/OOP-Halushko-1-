using System;
using System.Net.Http;
using System.Text;
using System.Threading;

public class RetryPolicy
{
    private int _maxRetries;
    private TimeSpan _initialDelay;
    private double _jitterFactor;

    public RetryPolicy(int maxRetries, TimeSpan initialDelay,
        double jitterFactor = 0.2)
    {
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation,
        Func<Exception, bool>? shouldRetry = null)
    {
        Random random = new Random();

        for (int attempt = 0; ; attempt++)
        {
            Console.WriteLine($"Спроба {attempt + 1}");

            try
            {
                T result = operation();
                Console.WriteLine("Успіх.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);

                if (attempt >= _maxRetries ||
                    (shouldRetry != null && !shouldRetry(ex)))
                    throw;

                double delay = _initialDelay.TotalMilliseconds
                    * Math.Pow(2, attempt)
                    * (1 + random.NextDouble() * _jitterFactor * 2
                        - _jitterFactor);

                Console.WriteLine($"Затримка: {delay:F0} мс");
                Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        RetryPolicy policy = new RetryPolicy(
            3, TimeSpan.FromMilliseconds(100));

        Console.WriteLine("1. Тимчасовий збій сервісу");
        int attempts = 0;

        string result = policy.Execute(() =>
        {
            attempts++;

            if (attempts < 3)
                throw new HttpRequestException(
                    "Сервіс тимчасово недоступний.");

            return "Дані отримано.";
        }, ex => ex is HttpRequestException);

        Console.WriteLine(result);

        Console.WriteLine("\n2. Постійний збій сервісу");

        try
        {
            policy.Execute<string>(() =>
            {
                throw new HttpRequestException("Сервіс не працює.");
            }, ex => ex is HttpRequestException);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Остаточна невдача: " + ex.Message);
        }
    }
}