using Mercora.Application.Dtos.Common;
using Mercora.Application.Dtos.Products;

namespace Mercora.Application.Products
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductListItemDto>> GetProductsAsync(ProductQueryDto q);
        Task<ProductDetailsDto?> GetProductBySlugAsync(string slug);
    }
}
