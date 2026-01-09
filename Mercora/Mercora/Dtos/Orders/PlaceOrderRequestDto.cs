namespace Mercora.Api.Dtos.Orders
{
    public class PlaceOrderRequestDto
    {
        public int UserId { get; init; }
        public IReadOnlyList<PlaceOrderLineDto> Lines { get; init; } = [];
    }
}
