using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Pinnacle.CustomerTeam.Caching.Services
{
    public class CacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get<T>(string sessionId, string key)
        {
            // Get from cache
            var cacheValue = await _cache.GetAsync(key);

            // If cache not found
            if (cacheValue == null) return default;

            // If cache  found
            var serializedResponse = Encoding.UTF8.GetString(cacheValue);
            var session = JsonConvert.DeserializeObject<T>(serializedResponse);
            
            // Return
            return session;
        }
        public async Task Add<T>(T session, string key, int expirationInMinutes)
        {
            // Add expiration
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(expirationInMinutes)
            };

            // Serialize
            var serializedResponse = JsonConvert.SerializeObject(session);

            // In bytes
            var responseInBytes = Encoding.UTF8.GetBytes(serializedResponse);

            // Set cache
            await _cache.SetAsync(key, responseInBytes, options);
        }
    }
}