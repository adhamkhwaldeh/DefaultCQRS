using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;


namespace AlJawad.DefaultCQRS.Caching.Mocks
{
    public class MockCachingService : IAppCache
    {
        public ICacheProvider CacheProvider { get; } = new MockCacheProvider();
        public CacheDefaults DefaultCachePolicy { get; set; }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public T GetOrAdd<T>(string key, Func<ICacheEntry, T> addItemFactory)
        {
            return addItemFactory(new MockCacheEntry(key));
        }

        public void Remove(string key)
        {
        }

        public Task<T> GetOrAddAsync<T>(string key, Func<ICacheEntry, Task<T>> addItemFactory)
        {
            return addItemFactory(new MockCacheEntry(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(default(T));
        }

        public void Add<T>(string key, T item, MemoryCacheEntryOptions policy)
        {
        }
    }
}
