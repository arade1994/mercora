using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercora.Infrastructure.Persistence;
using Mercora.Api.Dtos.Products;
using Mercora.Api.Dtos.Common;

namespace Mercora.Api.Controllers
{
    public class ProductsController(MercoraDbContext db) : ControllerBase
    {
        private readonly MercoraDbContext _db = db;

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<PagedResultDto<ProductListItemDto>>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var baseQuery = _db.Products
                .Where(product => product.IsPublished && !product.IsDeleted);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderByDescending(product => product.CreatedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(product => new ProductListItemDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Slug = product.Slug,
                    BasePrice = product.BasePrice,
                    CurrencyCode = product.CurrencyCode
                })
                .ToListAsync();

            return Ok(new PagedResultDto<ProductListItemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            });
        }

        [HttpGet("api/[controller]/{Slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            var normalized = slug.Trim().ToLowerInvariant();

            var product = await _db.Products
                .Where(product => product.Slug.ToLower() == normalized)
                .Select(product => new ProductDetailsDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Slug = product.Slug,
                    Description = product.Description,
                    BasePrice = product.BasePrice,
                    CurrencyCode = product.CurrencyCode
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
