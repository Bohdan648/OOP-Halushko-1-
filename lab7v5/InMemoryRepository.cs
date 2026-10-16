using System;
using System.Collections.Generic;
using System.Linq;

namespace lab7v5
{
    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> _items = new();

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_items.Any(x => x.Id == item.Id))
                throw new InvalidOperationException($"Елемент з ID {item.Id} вже існує.");

            _items.Add(item);
        }

        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var index = _items.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                throw new KeyNotFoundException($"Елемент з ID {item.Id} не знайдено.");

            _items[index] = item;
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item == null)
                throw new KeyNotFoundException($"Елемент з ID {id} не знайдено.");

            _items.Remove(item);
        }
    }
}