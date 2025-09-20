using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AlJawad.DefaultCQRS.Caching.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlJawad.DefaultCQRS.Caching
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddCustomMemoryCache(this IServiceCollection services)
        {

            services.TryAddTransient<IMemoryCache, MemoryCache>();
            services.TryAddTransient<ICacheProvider, MemoryCacheProvider>();
            services.TryAddTransient<IAppCache, CachingService>();
            return services;
        }

        #region List Cache
        public static async Task<IEnumerable<T>> GetListCacheValueAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            string result = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            //var deserializedObj = JsonSerializer.Deserialize<IEnumerable<T>>(result);
            var deserializedObj = JsonConvert.DeserializeObject<IEnumerable<T>>(result);
            return deserializedObj;
        }

        public static async Task SetListCacheValueAsync<T>(this IDistributedCache cache, string key, IEnumerable<T> value, int cachedMinutes = 20) where T : class
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                     .SetSlidingExpiration(TimeSpan.FromMinutes(cachedMinutes));
            //var result = JsonSerializer.Serialize(value);
            var result = JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(key, result, cacheEntryOptions);
        }

        public static async Task<IEnumerable<T>> GetAndCacheList<T>(this IDistributedCache cache, string key, Func<Task<IEnumerable<T>>> backup, int cachedMinutes = 20) where T : class
        {
            var item = await cache.GetListCacheValueAsync<T>(key).ConfigureAwait(false);
            if (item == null)
            {
                item = await backup();
                _ = cache.SetCacheValueAsync(key, item, cachedMinutes);
            }
            return item;
        }
        #endregion
       
        #region Single Cache
        public static async Task<T> GetCacheValueAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            string result = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
           // var deserializedObj = JsonSerializer.Deserialize<T>(result);
            var deserializedObj = JsonConvert.DeserializeObject<T>(result);
            return deserializedObj;
        }

        public static async Task SetCacheValueAsync<T>(this IDistributedCache cache, string key, T value, int cachedMinutes = 20) where T : class
        {
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                     .SetSlidingExpiration(TimeSpan.FromMinutes(cachedMinutes));
            try
            {
                //var result = JsonSerializer.Serialize(value);
                var result = JsonConvert.SerializeObject(value);
                var x = result.ToString();
                await cache.SetStringAsync(key, result, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }
        public static async Task<T> GetAndCache<T>(this IDistributedCache cache, string cacheKey, Func<Task<T>> backup, int cachedMinutes = 20)
            where T : class
        {
            var item = await cache.GetCacheValueAsync<T>(cacheKey).ConfigureAwait(false);
            if (item == null)
            {
                item = await backup();
                _ = cache.SetCacheValueAsync(cacheKey, item, cachedMinutes);
            }
            return item;
        }

        #endregion

    }
}
