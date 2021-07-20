using System.Collections.Generic;

namespace CartApi.Models.Responses
{
    public class GetResponse
    {
        public List<CartProductModel> CartProducts { get; set; } = null!;
    }
}