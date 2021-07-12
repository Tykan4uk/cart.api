using System.Collections.Generic;

namespace CartApi.Models.Responses
{
    public class GetCartResponse
    {
        public HashSet<int> GameIdList { get; set; } = null!;
    }
}