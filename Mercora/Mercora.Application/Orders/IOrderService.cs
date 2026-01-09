using Mercora.Application.Dtos.Orders;

namespace Mercora.Application.Orders
{
    public interface IOrderService
    {
        Task<PlaceOrderResponseDto> PlaceOrderAsync(PlaceOrderRequestDto request);
    }
}
