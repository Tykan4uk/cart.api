using System.Threading.Tasks;
using CartApi.Configuration;
using CartApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CartApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CartBffController : ControllerBase
    {
        private readonly ILogger<CartBffController> _logger;
        private readonly ICartService _cartService;
        private readonly Config _config;

        public CartBffController(
            ILogger<CartBffController> logger,
            IOptions<Config> config,
            ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
            _config = config.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Get(int userId)
        {
            var result = await _cartService.GetAsync(userId);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int userId, int gameId)
        {
            var result = await _cartService.AddAsync(userId, gameId);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int userId, int gameId)
        {
            var result = await _cartService.RemoveAsync(userId, gameId);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}