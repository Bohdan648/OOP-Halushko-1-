
using System;
using System.Collections.Generic;

class OrderNotFoundException : Exception
{
    public int OrderId { get; }

    public OrderNotFoundException() { }

    public OrderNotFoundException(string message) : base(message) { }

    public OrderNotFoundException(string message, Exception inner)
        : base(message, inner) { }

    public OrderNotFoundException(int orderId)
        : base("Замовлення з ID " + orderId + " не знайдено")
    {
        OrderId = orderId;
    }
}

class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, "");
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default, false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return Failure(ex.Message);
    }
}

class Order
{
    public int Id { get; set; }
    public string Product { get; set; } = "";
    public bool InStock { get; set; }
}

class OrderService
{
    private List<Order> orders = new List<Order>();

    public Order GetOrder(int orderId)
    {
        foreach (Order order in orders)
        {
            if (order.Id == orderId)
                return order;
        }

        throw new OrderNotFoundException(orderId);
    }

    public Result<bool> PlaceOrder(Order order)
    {
        if (!order.InStock)
            return Result<bool>.Failure("Товару немає в наявності");

        orders.Add(order);
        return Result<bool>.Success(true);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        OrderService service = new OrderService();

        Order order1 = new Order
        {
            Id = 1,
            Product = "Ноутбук",
            InStock = true
        };

        Order order2 = new Order
        {
            Id = 2,
            Product = "Телефон",
            InStock = false
        };

        Console.WriteLine("Оформлення замовлень:");

        Result<bool> result1 = service.PlaceOrder(order1);

        if (result1.IsSuccess)
            Console.WriteLine("Замовлення 1 оформлено");
        else
            Console.WriteLine(result1.Error);

        Result<bool> result2 = service.PlaceOrder(order2);

        if (result2.IsSuccess)
            Console.WriteLine("Замовлення 2 оформлено");
        else
            Console.WriteLine(result2.Error);

        Console.WriteLine("\nПошук замовлень:");

        try
        {
            Order found = service.GetOrder(1);
            Console.WriteLine("Знайдено: " + found.Product);

            service.GetOrder(5);
        }
        catch (OrderNotFoundException ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }
}
