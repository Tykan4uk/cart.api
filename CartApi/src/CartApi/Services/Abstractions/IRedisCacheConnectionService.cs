using StackExchange.Redis;

namespace CartApi.Services.Abstractions
{
    public interface IRedisCacheConnectionService
    {
        public IConnectionMultiplexer Connection { get; }
    }
}