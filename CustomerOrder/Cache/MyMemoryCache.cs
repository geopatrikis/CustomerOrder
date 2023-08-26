using Microsoft.Extensions.Caching.Memory;

namespace CustomerOrder.Cache
{
    public class MyMemoryCache:IMemoryCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 1024
            });

        public ICacheEntry CreateEntry(object key)
        {
            return Cache.CreateEntry(key);
        }

        public void Dispose()
        {
            Cache.Dispose();
        }

        public void Remove(object key)
        {
            Cache.Remove(key);
        }
        public bool TryGetValue(object key, out object? value)
        {
            return Cache.TryGetValue(key, out value);
        }
    }
}
