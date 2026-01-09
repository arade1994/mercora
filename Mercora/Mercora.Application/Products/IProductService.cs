using System;
using System.Collections.Generic;
using System.Text;

namespace Mercora.Application.Products
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductListItemDto>> GetProductsAsync(ProductQueryDto q);
        Task<ProductDetailsDto?> GetProductBySlugAsync(string slug);
    }
}
