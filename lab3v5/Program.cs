using System;

namespace ResourceManagementLab
{
    public class GraphicsContext : IDisposable
    {
        private int _contextId;
        private bool _isContextCreated;
        private bool _disposed = false;

        public int ContextId => _contextId;
        public bool IsContextCreated => _isContextCreated;

        public GraphicsContext(int contextId)
        {
            _contextId = contextId;
            _isContextCreated = true;
            Console.WriteLine($"[Конструктор]: Графічний контекст (ID: {_contextId}) успішно створено.");
        }

        public void DrawShape(string shape)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(GraphicsContext), "Неможливо намалювати: графічний контекст знищено.");

            if (_isContextCreated)
            {
                Console.WriteLine($"[Малювання]: Фігура '{shape}' намальована в контексті (ID: {_contextId}).");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"[Dispose(true)]: Звільнення керованих ресурсів контексту ID: {_contextId}.");
                }

                if (_isContextCreated)
                {
                    Console.WriteLine($"[Dispose]: Графічний контекст (ID: {_contextId}) знищено.");
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
            Console.WriteLine($"[Деструктор]: Викликано деструктор для контексту ID: {_contextId}.");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("==================================================");
            Console.WriteLine("СЦЕНАРІЙ 1: Використання оператора 'using'");
            Console.WriteLine("==================================================");
            using (var context1 = new GraphicsContext(101))
            {
                context1.DrawShape("Коло");
            }

            Console.WriteLine("\n==================================================");
            Console.WriteLine("СЦЕНАРІЙ 2: Явний виклик Dispose()");
            Console.WriteLine("==================================================");
            var context2 = new GraphicsContext(202);
            context2.DrawShape("Квадрат");
            context2.Dispose();

            Console.WriteLine("\n==================================================");
            Console.WriteLine("СЦЕНАРІЙ 3: Демонстрація роботи деструктора через GC.Collect()");
            Console.WriteLine("==================================================");
            CreateAndAbandonContext();

            Console.WriteLine("Викликаємо GC.Collect()...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nЗавершення роботи програми.");
        }

        static void CreateAndAbandonContext()
        {
            var context3 = new GraphicsContext(303);
            context3.DrawShape("Трикутник");
        }
    }
}