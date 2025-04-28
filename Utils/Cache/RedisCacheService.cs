using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Utils.Cache
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        // Lưu trữ dữ liệu vào Redis (Distributed Cache)
        public async Task SetRedisCacheAsync(string key, string value, TimeSpan? absoluteExpiration = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromMinutes(5)
                };
                await _distributedCache.SetStringAsync(key, value, options);
                _logger.LogInformation($"Redis Cache set for key: {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting Redis cache for key {key}: {ex.Message}");
            }
        }

        // Lấy dữ liệu từ Redis (Distributed Cache)
        public async Task<string> GetRedisCacheAsync(string key)
        {
            try
            {
                return await _distributedCache.GetStringAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Redis cache for key {key}: {ex.Message}");
                return null;
            }
        }
    }
}
