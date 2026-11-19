using System;
using System.Net.Http;
using System.Text;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;

class OutOfStockException : Exception
{
    public int RequestedQuantity { get; set; }
    public int AvailableQuantity { get; set; }

    public OutOfStockException() { }

    public OutOfStockException(string message) : base(message) { }

    public OutOfStockException(string message, Exception inner)
        : base(message, inner) { }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            int requested = 5;
            int available = 2;

            if (requested > available)
                throw new OutOfStockException("Недостатньо товару на складі.")
                {
                    RequestedQuantity = requested,
                    AvailableQuantity = available
                };
        }
        catch (OutOfStockException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(
                $"Запитано: {ex.RequestedQuantity}, є: {ex.AvailableQuantity}");
        }

        RetryDemo();
        CircuitBreakerDemo();
    }

    static void RetryDemo()
    {
        Console.WriteLine("\n1. Сервіс доставки: Retry");
        int attempt = 0;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMilliseconds(200)
            })
            .Build();

        pipeline.Execute(() =>
        {
            attempt++;
            Console.WriteLine($"Спроба {attempt}");

            if (attempt < 3)
            {
                Console.WriteLine("Помилка: сервіс доставки недоступний.");
                throw new HttpRequestException("Тимчасовий збій.");
            }

            Console.WriteLine("Дані відділень доставки отримано.");
        });
    }

    static void CircuitBreakerDemo()
    {
        Console.WriteLine("\n2. Платіжний сервіс: Circuit Breaker");

        var pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),
                FailureRatio = 1,
                MinimumThroughput = 2,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(5)
            })
            .Build();

        for (int i = 1; i <= 4; i++)
        {
            Console.WriteLine($"Запит {i}");

            try
            {
                pipeline.Execute(() =>
                {
                    Console.WriteLine("Звернення до платіжного сервісу.");
                    throw new HttpRequestException(
                        "Платіжний сервіс недоступний.");
                });
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine(
                    "Запит заблоковано відкритим Circuit Breaker.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }
}