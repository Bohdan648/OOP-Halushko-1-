using System;

public class GraphicsContext : IDisposable
{
    private int _contextId;
    private bool _isContextCreated;
    private bool _disposed;

    public GraphicsContext(int contextId)
    {
        _contextId = contextId;
        _isContextCreated = true;
        Console.WriteLine($"Конструктор: контекст {_contextId} створено.");
    }

    public void DrawShape(string shape)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(GraphicsContext), "Контекст знищено.");

        if (_isContextCreated)
            Console.WriteLine($"Малювання: {shape} (ID: {_contextId}).");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                Console.WriteLine($"Dispose(true): керовані ресурси (ID: {_contextId}).");

            if (_isContextCreated)
            {
                Console.WriteLine($"Dispose: контекст {_contextId} знищено.");
                _isContextCreated = false;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~GraphicsContext()
    {
        Console.WriteLine($"Деструктор: контекст {_contextId}.");
        Dispose(false);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("\n--- Сценарій 1: using ---");
        using (var c1 = new GraphicsContext(101))
        {
            c1.DrawShape("Коло");
        }

        Console.WriteLine("\n--- Сценарій 2: Явний Dispose ---");
        var c2 = new GraphicsContext(202);
        c2.DrawShape("Квадрат");
        c2.Dispose();

        Console.WriteLine("\n--- Сценарій 3: Деструктор + GC ---");
        CreateAndAbandon();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    static void CreateAndAbandon()
    {
        var c3 = new GraphicsContext(303);
        c3.DrawShape("Трикутник");
    }
}