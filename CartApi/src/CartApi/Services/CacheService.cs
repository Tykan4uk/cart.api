using System;
using System.Threading.Tasks;
using CartApi.Configuration;
using CartApi.Data.Cache;
using StackExchange.Redis;
using CartApi.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CartApi.Services
{
    public class CacheService<TCacheEntity> : ICacheService<TCacheEntity>
        where TCacheEntity : class, ICacheEntity
    {
        private readonly ILogger<CacheService<TCacheEntity>> _logger;
        private readonly IRedisCacheConnectionService _redisCacheConnectionService;
        private readonly IJsonSerializer _jsonSerializer;

        public CacheService(
            ILogger<CacheService<TCacheEntity>> logger,
            IRedisCacheConnectionService redisCacheConnectionService,
            IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _redisCacheConnectionService = redisCacheConnectionService;
            _jsonSerializer = jsonSerializer;
        }

        public Task AddOrUpdateAsync(TCacheEntity entity) => AddOrUpdateInternalAsync(entity);

        public async Task<TCacheEntity> GetAsync(string userId)
        {
            var redis = GetRedisDatabase();

            var cacheKey = GetItemCacheKey(userId);

            var serialized = await redis.StringGetAsync(cacheKey);

            return !string.IsNullOrEmpty(serialized) ? _jsonSerializer.Deserialize<TCacheEntity>(serialized) : null;
        }

        private string GetItemCacheKey(string userId) =>
            $"{userId}";

        private async Task AddOrUpdateInternalAsync(TCacheEntity entity, IDatabase redis = null, TimeSpan? expiry = null)
        {
            redis = redis ?? GetRedisDatabase();

            var cacheKey = GetItemCacheKey(entity.UserId);
            var serialized = _jsonSerializer.Serialize(entity);

            if (await redis.StringSetAsync(cacheKey, serialized, expiry))
            {
                _logger.LogInformation($"{typeof(TCacheEntity).Name} for user {entity.UserId} cached. New data: {serialized}");
            }
            else
            {
                _logger.LogInformation($"{typeof(TCacheEntity).Name} for user {entity.UserId} updated. New data: {serialized}");
            }
        }

        private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
    }
}