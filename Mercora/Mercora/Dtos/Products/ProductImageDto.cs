namespace Mercora.Api.Dtos.Products
{
    public class ProductImageDto
    {
        public int ImageId { get; init; }
        public string Url { get; init; } = null!;
        public bool IsPrimary { get; init; }
        public int SortOrder { get; init; }
    }
}
