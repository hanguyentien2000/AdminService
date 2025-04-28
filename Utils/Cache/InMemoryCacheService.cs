using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Utils.Cache
{
    public class InMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<InMemoryCacheService> _logger;

        public InMemoryCacheService(IMemoryCache memoryCache, ILogger<InMemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        // Lưu trữ dữ liệu vào In-Memory Cache
        public void SetMemoryCache(string key, string value, TimeSpan expiration)
        {
            try
            {
                _memoryCache.Set(key, value, expiration);
                _logger.LogInformation($"In-Memory Cache set for key: {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting In-Memory cache for key {key}: {ex.Message}");
            }
        }

        // Lấy dữ liệu từ In-Memory Cache
        public string GetMemoryCache(string key)
        {
            try
            {
                return _memoryCache.TryGetValue(key, out string cachedValue) ? cachedValue : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting In-Memory cache for key {key}: {ex.Message}");
                return null;
            }
        }
    }
}
