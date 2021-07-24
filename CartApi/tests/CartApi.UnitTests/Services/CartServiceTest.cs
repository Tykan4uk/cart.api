using System.Threading.Tasks;
using CartApi.Common.Enums;
using CartApi.Data.Cache;
using CartApi.Services;
using CartApi.Services.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CartApi.UnitTests.Services
{
    public class CartServiceTest
    {
        private readonly ICartService _cartService;
        private readonly Mock<ICacheService<CartCacheEntity>> _cacheService;

        private readonly CartCacheEntity _cartCacheEntitySuccess = new CartCacheEntity()
        {
            UserId = "testUserId"
        };
        private readonly CartCacheEntity _cartCacheEntityFailed = new CartCacheEntity()
        {
        };

        private readonly string _testNameSuccess = "testNameSuccess";
        private readonly string _testDescriptionSuccess = "testDescriptionSuccess";
        private readonly string _testUserIdSuccess = "testUserIdSuccess";
        private readonly string _testProductIdSuccess = "testProductIdSuccess";
        private readonly decimal _testPriceSuccess = 10.0M;

        private readonly string _testNameFailed = "testNameFailed";
        private readonly string _testDescriptionFailed = "testDescriptionFailed";
        private readonly string _testUserIdFailed = "testUserIdFailed";
        private readonly string _testProductIdFailed = "testProductIdFailed";
        private readonly decimal _testPriceFailed = 0.0M;

        public CartServiceTest()
        {
            _cacheService = new Mock<ICacheService<CartCacheEntity>>();

            _cacheService.Setup(s => s.GetAsync(
                It.Is<string>(i => i.Contains("testName")))).ReturnsAsync(_cartCacheEntitySuccess);

            _cacheService.Setup(s => s.GetAsync(
                It.Is<string>(i => i.Contains("empty")))).ReturnsAsync(_cartCacheEntityFailed);

            _cartService = new CartService(_cacheService.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange

            // act
            var result = await _cartService.AddAsync(_testUserIdSuccess, _testProductIdSuccess, _testNameSuccess, _testDescriptionSuccess, _testPriceSuccess, ProductTypeEnum.Game);

            // assert
            result.IsAdded.Should().BeTrue();
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange

            // act
            var result = await _cartService.AddAsync(_testUserIdFailed, _testProductIdFailed, _testNameFailed, _testDescriptionFailed, _testPriceFailed, ProductTypeEnum.Game);

            // assert
            result.IsAdded.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveAsync_Success()
        {
            // arrange

            // act
            var result = await _cartService.RemoveAsync(_testUserIdSuccess, _testProductIdSuccess);

            // assert
            result.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveAsync_Failed()
        {
            // arrange

            // act
            var result = await _cartService.RemoveAsync(_testUserIdFailed, _testProductIdFailed);

            // assert
            result.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            // arrange

            // act
            var result = await _cartService.GetAsync(_testUserIdSuccess);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_Failed()
        {
            // arrange

            // act
            var result = await _cartService.GetAsync(_testUserIdFailed);

            // assert
            result.Should().BeNull();
        }
    }
}
