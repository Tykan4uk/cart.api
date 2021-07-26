using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartApi.Common.Enums;
using CartApi.Data.Cache;
using CartApi.Models;
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

        public async Task<AddResponse> AddAsync(string userId, string productId, string name, string description, decimal price, ProductTypeEnum productType)
        {
            var cartProduct = new CartProductModel()
            {
                Id = productId,
                Name = name,
                Description = description,
                Price = price,
                Type = productType,
                Count = 1
            };
            var cache = await _cacheService.GetAsync(userId);
            if (cache == null)
            {
                await _cacheService.AddOrUpdateAsync(new CartCacheEntity()
                {
                    UserId = userId,
                    CartProducts = new List<CartProductModel>() { cartProduct }
                });
            }
            else
            {
                var product = cache.CartProducts.FirstOrDefault(f => f.Id == productId);
                if (product == null)
                {
                    cache.CartProducts.Add(cartProduct);
                }
                else
                {
                    product.Count++;
                }

                await _cacheService.AddOrUpdateAsync(cache);
            }

            return new AddResponse() { IsAdded = true };
        }

        public async Task<GetResponse> GetAsync(string userId)
        {
            var cache = await _cacheService.GetAsync(userId);

            return cache != null ? new GetResponse() { CartProducts = cache.CartProducts } : null;
        }

        public async Task<RemoveResponse> RemoveAsync(string userId, string productId)
        {
            var cache = await _cacheService.GetAsync(userId);
            if (cache != null)
            {
                var removeProduct = cache.CartProducts.FirstOrDefault(f => f.Id == productId);
                if (removeProduct == null)
                {
                    return new RemoveResponse() { IsDeleted = false };
                }

                removeProduct.Count--;

                if (removeProduct.Count <= 0)
                {
                    cache.CartProducts.Remove(removeProduct);
                }

                await _cacheService.AddOrUpdateAsync(cache);
            }
            else
            {
                return new RemoveResponse() { IsDeleted = false };
            }

            return new RemoveResponse() { IsDeleted = true };
        }

        public async Task<ClearResponse> ClearAsync(string userId)
        {
            var cache = await _cacheService.GetAsync(userId);
            if (cache != null)
            {
                cache = null;
            }

            return new ClearResponse() { IsCleared = true };
        }
    }
}