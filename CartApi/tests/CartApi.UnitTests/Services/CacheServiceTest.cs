﻿using System;
using System.Threading.Tasks;
using CartApi.Configuration;
using CartApi.Data.Cache;
using CartApi.Services;
using CartApi.Services.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace CartApi.UnitTests.Services
{
    public class CacheServiceTest
    {
        private readonly ICacheService<CartCacheEntity> _cacheService;

        private readonly Mock<IOptions<Config>> _config;
        private readonly Mock<ILogger<CacheService<CartCacheEntity>>> _logger;
        private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionService;
        private readonly Mock<IJsonSerializer> _jsonSerializer;

        public CacheServiceTest()
        {
            _config = new Mock<IOptions<Config>>();
            _logger = new Mock<ILogger<CacheService<CartCacheEntity>>>();

            _redisCacheConnectionService = new Mock<IRedisCacheConnectionService>();
            var connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            var redisDataBase = new Mock<IDatabase>();

            redisDataBase.Setup(expression: x => x.StringSetAsync(
                    It.IsAny<RedisKey>(),
                    It.IsAny<RedisValue>(),
                    It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(),
                    It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            connectionMultiplexer
                .Setup(x => x.GetDatabase(
                    It.IsAny<int>(),
                    It.IsAny<object>()))
                .Returns(redisDataBase.Object);

            _redisCacheConnectionService
                .Setup(x => x.Connection)
                .Returns(connectionMultiplexer.Object);

            _jsonSerializer = new Mock<IJsonSerializer>();

            _cacheService =
                new CacheService<CartCacheEntity>(
                    _logger.Object,
                    _redisCacheConnectionService.Object,
                    _config.Object,
                    _jsonSerializer.Object);
        }

        [Fact]
        public async Task AddOrUpdateAsync_Success()
        {
            // arrange
            var testEntity = new CartCacheEntity()
            {
                UserId = "TestUserId"
            };

            // act
            await _cacheService.AddOrUpdateAsync(testEntity);

            // assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()
                        .Contains("cached. New data:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateAsync_Failed()
        {
            // arrange
            var testEntity = new CartCacheEntity()
            {
                UserId = null
            };

            // act
            await _cacheService.AddOrUpdateAsync(testEntity);

            // assert
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()
                        .Contains("update. New data:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            // arrange
            var testName = "testName";

            // act
            var result = await _cacheService.GetAsync(testName);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_Failed()
        {
            // arrange
            var testName = "testName";

            // act
            var result = await _cacheService.GetAsync(testName);

            // assert
            result.Should().BeNull();
        }
    }
}
