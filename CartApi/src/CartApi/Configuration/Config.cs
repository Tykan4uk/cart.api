namespace CartApi.Configuration
{
    public class Config
    {
        public CartApiConfig CartApi { get; set; } = null!;
        public RedisConfig Redis { get; set; } = null!;
    }
}