using Mercora.Application.Dtos.Common;
using Mercora.Application.Dtos.Products;

namespace Mercora.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<PagedResultDto<ProductListItemDto>> GetProductsAsync(ProductQueryDto q);
        Task<ProductDetailsDto?> GetBySlugAsync(string slug);
    }
}
