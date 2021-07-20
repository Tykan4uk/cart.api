using System.Collections.Generic;
using CartApi.Models;

namespace CartApi.Data.Cache
{
    public class CartCacheEntity : ICacheEntity
    {
        public string UserId { get; set; }
        public List<CartProductModel> CartProducts { get; set; } = null!;
    }
}