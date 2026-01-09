using System.Data;
using Mercora.Api.Dtos.Orders;
using Mercora.Infrastructure.Persistence;
using Mercora.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Mercora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MercoraDbContext _db;

        public OrdersController(MercoraDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequestDto request)
        {
            if (request.UserId <= 0)
                return BadRequest("User id is required");

            if (request.Lines is null || request.Lines.Count == 0)
                return BadRequest("Order must contain at least one line");

            if (request.Lines.Any(l => l.VariantId <= 0 || l.Quantity <= 0))
                return BadRequest("Each line must have valid VariantId and Quantity > 0");

            if (request.Lines.GroupBy(l => l.VariantId).Any(g => g.Count() > 1))
                return BadRequest("Duplicate VariantId lines are not allowed");

            var tvp = SqlTvpFactory.BuildOrderLinesTvp(
                request.Lines.Select(l => (l.VariantId, l.Quantity)));

            var userIdParam = new SqlParameter("@UserId", request.UserId);
            var linesParam = new SqlParameter("Lines", tvp)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.OrderLineType"
            };
            var orderIdParam = new SqlParameter("@OrderId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var orderNumberParam = new SqlParameter("@OrderNumber", SqlDbType.NVarChar, 30)
            {
                Direction = ParameterDirection.Output
            };

            try
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.spPlaceOrder @UserId, @Lines, @OrderId OUTPUT, @OrderNumber OUTPUT",
                    userIdParam, linesParam, orderIdParam, orderNumberParam);

                var orderId = (int)orderIdParam.Value;
                var orderNumber = (int)orderIdParam.Value;

                var order = await _db.Orders
                    .Where(o => o.OrderId == orderId)
                    .Select(o => new { o.Subtotal, o.CurrencyCode })
                    .FirstAsync();

                return Ok(new PlaceOrderResponseDto
                {
                    OrderId = orderId,
                    OrderNumber = orderNumber.ToString(),
                    TotalAmount = order.Subtotal,
                    CurrencyCode = order.CurrencyCode
                });
            } 
            catch (SqlException ex) when (ex.Number >= 50000 && ex.Number < 60000) 
            {
                return BadRequest(new { errorCode = ex.Number, message = ex.Message });
            }
        }
    }
}
