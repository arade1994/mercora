using Mercora.Application.Abstractions.Persistence;
using Mercora.Application.Dtos.Common;
using Mercora.Application.Dtos.Products;
using Mercora.Application.Products;

namespace Mercora.Infrastructure.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _products;

        public ProductService(IProductRepository products) => _products = products;

        public async Task<PagedResultDto<ProductListItemDto>> GetProductsAsync(ProductQueryDto q)
        {
            return await _products.GetProductsAsync(q);
        }

        public async Task<ProductDetailsDto?> GetProductBySlugAsync(string slug)
        {
            var normalized = slug.Trim().ToLowerInvariant();

            return await _products.GetBySlugAsync(normalized);
        }
    }
}
