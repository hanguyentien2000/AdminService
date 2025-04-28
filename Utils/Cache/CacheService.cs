using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Cache
{
    public class CacheService
    {
        private readonly InMemoryCacheService _inMemoryCacheService;
        private readonly RedisCacheService _redisCacheService;

        public CacheService(InMemoryCacheService inMemoryCacheService, RedisCacheService redisCacheService)
        {
            _inMemoryCacheService = inMemoryCacheService;
            _redisCacheService = redisCacheService;
        }

        // Sử dụng Redis Cache
        public async Task SetCacheAsync(string key, string value, TimeSpan? expiration = null)
        {
            // Thử lưu vào Redis Cache trước
            await _redisCacheService.SetRedisCacheAsync(key, value, expiration);
        }

        public async Task<string> GetCacheAsync(string key)
        {
            // Thử lấy từ Redis Cache trước
            var cachedValue = await _redisCacheService.GetRedisCacheAsync(key);
            if (string.IsNullOrEmpty(cachedValue))
            {
                // Nếu không có trong Redis, kiểm tra In-Memory Cache
                cachedValue = _inMemoryCacheService.GetMemoryCache(key);
            }
            return cachedValue;
        }

        // Khởi tạo cache nếu không có dữ liệu
        public async Task<string> InitCacheAsync(string key, Func<Task<string>> fetchDataFunc)
        {
            var cachedValue = await GetCacheAsync(key);

            if (string.IsNullOrEmpty(cachedValue))
            {
                cachedValue = await fetchDataFunc();
                if (!string.IsNullOrEmpty(cachedValue))
                {
                    // Lưu vào Redis Cache
                    await SetCacheAsync(key, cachedValue);
                }
            }

            return cachedValue;
        }
    }
}
