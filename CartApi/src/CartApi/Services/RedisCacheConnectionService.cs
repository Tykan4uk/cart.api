using System;
using CartApi.Configuration;
using CartApi.Services.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CartApi.Services
{
    public class RedisCacheConnectionService : IRedisCacheConnectionService, IDisposable
    {
        private readonly Lazy<ConnectionMultiplexer> _connectionLazy;
        private bool _disposed;

        public RedisCacheConnectionService(
            IOptions<Config> config)
        {
            var redisConfigurationOptions = ConfigurationOptions.Parse(config.Value.Redis.Host);
            _connectionLazy =
                new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisConfigurationOptions));
        }

        public IConnectionMultiplexer Connection => _connectionLazy.Value;

        public void Dispose()
        {
            if (!_disposed)
            {
                Connection.Dispose();
                _disposed = true;
            }
        }
    }
}