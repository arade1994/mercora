namespace Mercora.Application.Dtos.Products
{
    public class ProductQueryDto
    {
        public string? Search { get; init; }
        public string? CategorySlug { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }

        // allowed values: newest, priceAsc, priceDesc, nameAsc, nameDesc
        public string? Sort { get; init; }

        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
