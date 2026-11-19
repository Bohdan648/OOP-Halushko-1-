using System;
using System.Collections.Generic;

namespace IndependentWork7
{
    public class Cache<TKey, TValue> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Queue<TKey> _keysOrder;

        public Cache(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.");

            _capacity = capacity;
            _cache = new Dictionary<TKey, TValue>(capacity);
            _keysOrder = new Queue<TKey>(capacity);
        }

        public int Count => _cache.Count;

        public void Add(TKey key, TValue value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
                return;
            }

            if (_cache.Count >= _capacity)
            {
                TKey oldestKey = _keysOrder.Dequeue();
                _cache.Remove(oldestKey);
            }

            _cache[key] = value;
            _keysOrder.Enqueue(key);
        }

        public TValue Get(TKey key)
        {
            if (_cache.TryGetValue(key, out TValue? value))
                return value;

            throw new KeyNotFoundException($"Key '{key}' not found.");
        }

        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var cache = new Cache<string, string>(3);
            cache.Add("1", "Перший");
            cache.Add("2", "Другий");
            cache.Add("3", "Третій");

            Console.WriteLine($"Кількість: {cache.Count}");
            Console.WriteLine($"Ключ 1: {cache.Get("1")}");

            cache.Add("4", "Четвертий");

            Console.WriteLine($"Кількість після додавання 4-го: {cache.Count}");
            Console.WriteLine($"Чи є ключ 1: {cache.ContainsKey("1")}");
            Console.WriteLine($"Чи є ключ 4: {cache.ContainsKey("4")}");
        }
    }
}