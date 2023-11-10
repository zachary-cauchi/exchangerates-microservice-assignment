using Microsoft.Extensions.Caching.Distributed;

namespace Exchange.API.Extensions
{
    /// <summary>
    /// Helper methods for dealing with redis cache.
    /// Taken from https://nishanc.medium.com/redis-as-a-distributed-cache-on-net-6-0-949ef5b795ee
    /// </summary>
    public static class DistribitutedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T record, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30);
            options.SlidingExpiration = slidingExpireTime;

            var jsonData = JsonSerializer.Serialize(record);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
