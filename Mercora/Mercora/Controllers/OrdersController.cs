using Mercora.Application.Dtos.Orders;
using Mercora.Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Mercora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orders;

        public OrdersController(IOrderService orders) => _orders = orders;

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequestDto request)
        {
            return Ok(await _orders.PlaceOrderAsync(request));
        }
    }
}
