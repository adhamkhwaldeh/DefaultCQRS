using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace AlJawad.DefaultCQRS.Caching
{
    public interface ICacheProvider
    {
        void Set(string key, object item, MemoryCacheEntryOptions policy);
        object Get(string key);
        object GetOrCreate<T>(string key, Func<ICacheEntry, T> func);
        void Remove(string key);
        Task<T> GetOrCreateAsync<T>(string key, Func<ICacheEntry, Task<T>> func);
    }
}
