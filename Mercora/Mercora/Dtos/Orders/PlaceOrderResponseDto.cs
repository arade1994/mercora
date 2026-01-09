namespace Mercora.Api.Dtos.Orders
{
    public class PlaceOrderResponseDto
    {
        public int OrderId { get; init; }
        public string OrderNumber { get; init; } = null!;
        public decimal TotalAmount { get; init; }
        public string CurrencyCode { get; init; } = null!;
    }
}
