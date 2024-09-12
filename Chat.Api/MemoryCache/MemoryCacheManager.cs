using Microsoft.Extensions.Caching.Memory;

namespace Chat.Api.MemoryCache
{
    public class MemoryCacheManager
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void UpdateValue(string key, object dtos)
        {
            _memoryCache.Set(key, dtos);
        }


        public object? GetDtos(string key)
        {
            if (_memoryCache.TryGetValue(key, out object dtos))
            {
                return dtos;
            }

            return null;
        }
    }
}
