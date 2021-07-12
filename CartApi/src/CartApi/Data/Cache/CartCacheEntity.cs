using System.Collections.Generic;

namespace CartApi.Data.Cache
{
    public class CartCacheEntity : ICacheEntity
    {
        public int UserId { get; set; }
        public HashSet<int> GameIdList { get; set; } = null!;
    }
}