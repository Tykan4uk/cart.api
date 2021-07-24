﻿using System.Threading.Tasks;
using CartApi.Models.Requests;
using CartApi.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CartApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize(Policy = "ApiScopeBff")]
    public class CartBffController : ControllerBase
    {
        private readonly ILogger<CartBffController> _logger;
        private readonly ICartService _cartService;

        public CartBffController(
            ILogger<CartBffController> logger,
            ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] GetRequest request)
        {
            var result = await _cartService.GetAsync(request.UserId);

            if (result == null)
            {
                _logger.LogInformation("(CartBffController/Get)Null result. Bad request.");
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
                _logger.LogInformation("(CartBffController/Add)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] RemoveRequest request)
        {
            var result = await _cartService.RemoveAsync(request.UserId, request.ProductId);

            if (result == null)
            {
                _logger.LogInformation("(CartBffController/Remove)Null result. Bad request.");
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}