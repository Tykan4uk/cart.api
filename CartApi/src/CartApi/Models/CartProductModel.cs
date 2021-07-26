using CartApi.Common.Enums;

namespace CartApi.Models
{
    public class CartProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductTypeEnum Type { get; set; }
        public int Count { get; set; }
    }
}
