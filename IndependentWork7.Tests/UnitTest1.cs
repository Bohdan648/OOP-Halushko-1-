using System.Collections.Generic;
using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_And_Get_ReturnsCorrectValue()
        {
            var cache = new Cache<int, string>(3);
            cache.Add(1, "One");
            cache.Add(2, "Two");

            Assert.Equal("One", cache.Get(1));
            Assert.Equal("Two", cache.Get(2));
        }

        [Fact]
        public void Add_ExceedingCapacity_RemovesOldestElement()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "One");
            cache.Add(2, "Two");
            cache.Add(3, "Three");

            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void Get_NonExistentKey_ThrowsKeyNotFoundException()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("a", 10);

            Assert.Throws<KeyNotFoundException>(() => cache.Get("b"));
        }

        [Fact]
        public void ContainsKey_ReturnsExpectedResult()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("key1", "val1");

            Assert.True(cache.ContainsKey("key1"));
            Assert.False(cache.ContainsKey("key2"));
        }

        [Fact]
        public void Count_ReturnsCurrentNumberOfItems()
        {
            var cache = new Cache<int, int>(3);
            Assert.Equal(0, cache.Count);

            cache.Add(1, 100);
            cache.Add(2, 200);
            Assert.Equal(2, cache.Count);
        }
    }
}