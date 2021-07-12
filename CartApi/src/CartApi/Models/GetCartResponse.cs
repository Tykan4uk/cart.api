using System.Collections.Generic;

namespace CartApi.Models
{
    public class GetCartResponse
    {
        public HashSet<int> GameIdList { get; set; } = null!;
    }
}