using StackExchange.Redis;

namespace CartApi.Services.Abstractions
{
    public interface IRedisCacheConnectionService
    {
        public ConnectionMultiplexer Connection { get; }
    }
}