using CartApi.Common.Enums;

namespace CartApi.Models.Requests
{
    public class AddRequest
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductTypeEnum Type { get; set; }
    }
}
