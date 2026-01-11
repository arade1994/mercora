using Mercora.Application.Dtos.Orders;

namespace Mercora.Application.Abstractions.Persistence
{
    public interface IOrderRepository
    {
        Task<PlaceOrderResponseDto> PlaceOrderAsync(PlaceOrderRequestDto request);
    }
}
