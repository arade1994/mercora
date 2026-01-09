namespace Mercora.Application.Dtos.Products
{
    public class ProductListItemDto
    {
        public int ProductId { get; init; }
        public string Name { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public decimal BasePrice { get; init; }
        public string CurrencyCode { get; init; } = null!;
    }
}
