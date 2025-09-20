using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Caching.Providers
{
    public class MemoryCacheProvider : ICacheProvider
    {
        internal readonly IMemoryCache cache;

        public MemoryCacheProvider(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void Set(string key, object item, MemoryCacheEntryOptions policy)
        {
            cache.Set(key, item, policy);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public object GetOrCreate<T>(string key, Func<ICacheEntry, T> factory)
        {
            return cache.GetOrCreate(key, factory);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> factory)
        {
            return cache.GetOrCreateAsync(key, factory);
        }
    }
}
