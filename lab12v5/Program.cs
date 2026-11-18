using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace lab12v5
{
    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int retryCount = 3,
            TimeSpan initialDelay = default,
            Func<Exception, bool>? shouldRetry = null)
        {
            if (initialDelay == default)
                initialDelay = TimeSpan.FromMilliseconds(300);

            int attempt = 0;
            while (true)
            {
                try
                {
                    attempt++;
                    return operation();
                }
                catch (Exception ex)
                {
                    bool canRetry = shouldRetry == null || shouldRetry(ex);
                    if (attempt > retryCount || !canRetry)
                    {
                        Console.WriteLine($"[Зупинка] Спроба {attempt}: {ex.Message}");
                        throw;
                    }

                    double delayMs = initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
                    Console.WriteLine($"[Спроба {attempt}] Помилка: {ex.Message}. Чекаємо {delayMs} мс...");
                    Thread.Sleep((int)delayMs);
                }
            }
        }
    }

    public class FileProcessor
    {
        private int _tries = 0;

        public void UpdateConfig(string path, string key, string value)
        {
            _tries++;
            if (_tries <= 2)
                throw new IOException("Файл тимчасово заблокований.");

            Console.WriteLine($"[Успіх] Файл {path} оновлено: {key} = {value}");
        }
    }

    public class NetworkClient
    {
        private int _tries = 0;

        public bool SendConfigUpdate(string url, string configJson)
        {
            _tries++;
            if (_tries <= 3)
                throw new HttpRequestException("Сервер не відповідає (503).");

            Console.WriteLine($"[Успіх] Дані відправлено на {url}: {configJson}");
            return true;
        }
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var file = new FileProcessor();
            var net = new NetworkClient();
            Func<Exception, bool> filter = ex => ex is IOException || ex is HttpRequestException;

            Console.WriteLine("=== 1. Оновлення файлу конфігурації ===");
            RetryHelper.ExecuteWithRetry(() =>
            {
                file.UpdateConfig("config.json", "Theme", "Dark");
                return true;
            }, 3, TimeSpan.FromMilliseconds(200), filter);

            Console.WriteLine("\n=== 2. Відправка конфігурації на сервер ===");
            RetryHelper.ExecuteWithRetry(
                () => net.SendConfigUpdate("https://api.site.com/cfg", "{\"Theme\":\"Dark\"}"),
                4, TimeSpan.FromMilliseconds(200), filter);

            Console.WriteLine("\n=== 3. Перевірка shouldRetry (інша помилка) ===");
            try
            {
                RetryHelper.ExecuteWithRetry<bool>(
                    () => throw new ArgumentException("Невірний параметр"),
                    3, TimeSpan.FromMilliseconds(200), filter);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("[Перехоплено] Повторів не було, бо це не IO/Http помилка.");
            }
        }
    }
}