using System;
using System.Collections.Generic;

namespace lab8v5
{
    public static class CollectionHelper<T>
    {
        public static Dictionary<TKey, List<T>> GroupBy<TKey>(
            IEnumerable<T> collection, 
            Func<T, TKey> keySelector) where TKey : notnull
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            var result = new Dictionary<TKey, List<T>>();

            foreach (var item in collection)
            {
                TKey key = keySelector(item);

                if (!result.ContainsKey(key))
                {
                    result[key] = new List<T>();
                }

                result[key].Add(item);
            }

            return result;
        }
    }
}