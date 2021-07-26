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
        public async Task<IActionResult> Get([FromQuery] GetRequest request)
        {
            var result = await _cartService.GetAsync(request.UserId);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/Get)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRequest request)
        {
            var result = await _cartService.AddAsync(request.UserId, request.ProductId, request.Name, request.Description, request.Price, request.Type);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/Add)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] RemoveRequest request)
        {
            var result = await _cartService.RemoveAsync(request.UserId, request.ProductId);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/Remove)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Clear([FromBody] ClearRequest request)
        {
            var result = await _cartService.ClearAsync(request.UserId);

            if (result == null)
            {
                _logger.LogInformation("(ManageController/Clear)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}