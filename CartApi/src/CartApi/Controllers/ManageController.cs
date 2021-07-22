using System.Threading.Tasks;
using CartApi.Models.Requests;
using CartApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CartApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize(Policy = "ApiScope")]
    public class ManageController : ControllerBase
    {
        private readonly ILogger<ManageController> _logger;
        private readonly ICartService _cartService;

        public ManageController(
            ILogger<ManageController> logger,
            ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetRequest request)
        {
            var result = await _cartService.GetAsync(request.UserId);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            var result = await _cartService.AddAsync(request.UserId, request.ProductId, request.Name, request.Description, request.Price, request.Type);
            return result != null ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] RemoveRequest request)
        {
            var result = await _cartService.RemoveAsync(request.UserId, request.ProductId);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}