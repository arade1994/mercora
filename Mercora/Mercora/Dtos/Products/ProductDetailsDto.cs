namespace Mercora.Api.Dtos.Products
{
    public class ProductDetailsDto
    {
        public int ProductId { get; init; }
        public string Name { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string? Description { get; init; }
        public decimal BasePrice { get; init;  }
        public string CurrencyCode { get; init; } = null!;
    }
}
