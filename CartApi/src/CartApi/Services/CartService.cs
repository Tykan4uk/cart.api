using System.Collections.Generic;
using System.Threading.Tasks;
using CartApi.Data.Cache;
using CartApi.Models.Responses;
using CartApi.Services.Abstractions;

namespace CartApi.Services
{
    public class CartService : ICartService
    {
        private readonly ICacheService<CartCacheEntity> _cacheService;

        public CartService(ICacheService<CartCacheEntity> cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<AddCartResponse> AddAsync(int userId, int gameId)
        {
            var cache = await _cacheService.GetAsync(userId);
            if (cache == null)
            {
                await _cacheService.AddOrUpdateAsync(new CartCacheEntity() { UserId = userId, GameIdList = new HashSet<int> { gameId } });
            }
            else
            {
                cache.GameIdList.Add(gameId);
                await _cacheService.AddOrUpdateAsync(cache);
            }

            return new AddCartResponse() { UserId = userId };
        }

        public async Task<GetCartResponse> GetAsync(int userId)
        {
            var cache = await _cacheService.GetAsync(userId);

            return cache != null ? new GetCartResponse() { GameIdList = cache.GameIdList } : null;
        }

        public async Task<RemoveCartResponse> RemoveAsync(int userId, int gameId)
        {
            var cache = await _cacheService.GetAsync(userId);
            if (cache != null)
            {
                cache.GameIdList.Remove(gameId);
                await _cacheService.AddOrUpdateAsync(cache);
            }
            else
            {
                return null;
            }

            return new RemoveCartResponse() { UserId = cache.UserId };
        }
    }
}