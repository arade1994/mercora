namespace Mercora.Application.Dtos.Products
{
    public class ProductDetailsDto
    {
        public int ProductId { get; init; }
        public string Name { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string? Description { get; init; }
        public decimal BasePrice { get; init;  }
        public string CurrencyCode { get; init; } = null!;
        public string? PrimaryImageUrl { get; init; }
        public IReadOnlyList<ProductImageDto> Images { get; init; } = [];
        public IReadOnlyList<ProductVariantDto> Variants { get; init; } = [];
    }
}
