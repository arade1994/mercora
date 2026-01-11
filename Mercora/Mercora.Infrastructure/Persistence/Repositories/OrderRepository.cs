using Mercora.Application.Abstractions.Persistence;
using Mercora.Application.Common.Exceptions;
using Mercora.Application.Dtos.Orders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mercora.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MercoraDbContext _db;

        public OrderRepository(MercoraDbContext db) => _db = db;

        public async Task<PlaceOrderResponseDto> PlaceOrderAsync(PlaceOrderRequestDto request)
        {
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
            }
            catch (SqlException ex) when (ex.Number >= 50000 && ex.Number <= 60000)
            {
                throw new BusinessRuleException(ex.Number, ex.Message);
            }

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
