using Mercora.Application.Dtos.Orders;
using Mercora.Application.Orders;
using Microsoft.EntityFrameworkCore;
using Mercora.Application.Abstractions.Persistence;

namespace Mercora.Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orders;

        public OrderService(IOrderRepository orders) => _orders = orders;

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

            return await _orders.PlaceOrderAsync(request);
        }
    }
}
