namespace Mercora.Application.Dtos.Products
{
    public class ProductVariantDto
    {
        public int VariantId { get; init; }
        public string Sku { get; init; } = null!;
        public string? VariantName { get; init; }
        public decimal Price { get; init; }
        public int QuantityOnHand { get; init; }
        public int ReorderPoint { get; init; }
        public bool IsActive { get; init; }
    }
}
