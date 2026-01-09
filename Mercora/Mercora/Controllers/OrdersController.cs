using Mercora.Application.Dtos.Orders;
using Mercora.Application.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
            try
            {
                var result = await _orders.PlaceOrderAsync(request);

                return Ok(result);
            }
            catch (SqlException ex) when (ex.Number >= 50000 && ex.Number < 60000)
            {
                return BadRequest(new { errorCode = ex.Number, message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
