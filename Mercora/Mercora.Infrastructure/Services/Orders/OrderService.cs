using Mercora.Application.Dtos.Orders;
using Mercora.Application.Orders;
using Mercora.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mercora.Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly MercoraDbContext _db;

        public OrderService(MercoraDbContext db) => _db = db;  

        public async Task<PlaceOrderResponseDto> PlaceOrderAsync(PlaceOrderRequestDto request)
        {
            if (request.UserId <= 0)
                throw new ArgumentException("UserId is required.", nameof(request.UserId));

            if (request.Lines is null || request.Lines.Count == 0)
                throw new ArgumentException("Order must contain at least one line", nameof(request.Lines));

            if (request.Lines.Any(l => l.VariantId <= 0 || l.Quantity <= 0))
                throw new ArgumentException("Each line must have valid VariantId and Quantity > 0", nameof(request.Lines));

            if (request.Lines.GroupBy(l => l.VariantId).Any(g => g.Count() > 1))
                throw new ArgumentException("Duplicate VariantId lines are not allowed", nameof(request.Lines));

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

            await _db.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.spPlaceOrder @UserId, @Lines, @OrderId OUTPUT, @OrderNumber OUTPUT",
                    userIdParam, linesParam, orderIdParam, orderNumberParam);

            var orderId = (int)orderIdParam.Value;
            var orderNumber = (int)orderIdParam.Value;

            var total = await _db.Orders
                .Where(o => o.OrderId == orderId)
                .Select(o => new { o.Subtotal, o.CurrencyCode })
                .FirstAsync();

            return new PlaceOrderResponseDto
            {
                OrderId = orderId,
                OrderNumber = orderNumber.ToString(),
                TotalAmount = total.Subtotal,
                CurrencyCode = total.CurrencyCode
            };
        }
    }
}
