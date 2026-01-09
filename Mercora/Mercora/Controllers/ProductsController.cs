using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercora.Infrastructure.Persistence;
using Mercora.Application.Dtos.Products;
using Mercora.Application.Dtos.Common;

namespace Mercora.Api.Controllers
{
    public class ProductsController(MercoraDbContext db) : ControllerBase
    {
        private readonly MercoraDbContext _db = db;

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<PagedResultDto<ProductListItemDto>>> GetProducts([FromQuery] ProductQueryDto q)
        {
            var page = q.Page < 1 ? 1 : q.Page;
            var pageSize = q.PageSize < 1 ? 20 : q.PageSize;
            if (pageSize > 100) pageSize = 100;

            var baseQuery = _db.Products
                .Where(product => product.IsPublished && !product.IsDeleted);

            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var search = q.Search.Trim();
                baseQuery = baseQuery.Where(product => product.Name.Contains(search));
            }

            if (q.MinPrice is not null)
                baseQuery = baseQuery.Where(product => product.BasePrice >= q.MinPrice);

            if (q.MaxPrice is not null)
                baseQuery = baseQuery.Where(product => product.BasePrice <= q.MaxPrice);
            
            if (!string.IsNullOrWhiteSpace(q.CategorySlug))
            {
                var slug = q.CategorySlug.Trim();

                baseQuery = baseQuery
                    .Where(product => product.Category
                    .Any(category => category.Slug == slug && category.IsActive));
            }

            baseQuery = (q.Sort ?? "newest").Trim() switch
            {
                "priceAsc" => baseQuery.OrderBy(p => p.BasePrice),
                "priceDesc" => baseQuery.OrderByDescending(p => p.BasePrice),
                "nameAsc" => baseQuery.OrderBy(p => p.Name),
                "nameDesc" => baseQuery.OrderByDescending(p => p.Name),
                _ => baseQuery.OrderByDescending(p => p.CreatedAtUtc)
            };

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
                .Where(product => product.Slug.ToLower() == normalized && product.IsPublished && !product.IsDeleted)
                .Select(product => new ProductDetailsDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Slug = product.Slug,
                    Description = product.Description,
                    BasePrice = product.BasePrice,
                    CurrencyCode = product.CurrencyCode,
                    PrimaryImageUrl = product.ProductImages != null ? product.ProductImages.Url : null,
                    Images = product.ProductImages != null
                        ? new List<ProductImageDto>
                        {
                            new ProductImageDto
                            {
                                ImageId = product.ProductImages.ImageId,
                                Url = product.ProductImages.Url,
                                IsPrimary = true,
                                SortOrder = 0
                            }
                        } 
                        : new List<ProductImageDto>(),
                    Variants = product.ProductVariants
                        .Where(variant => variant.IsActive)
                        .OrderBy(variant => variant.Sku)
                        .Select(variant => new ProductVariantDto
                        {
                            VariantId = variant.VariantId,
                            Sku = variant.Sku,
                            VariantName = variant.VariantName,
                            Price = product.BasePrice + variant.PriceDelta,
                            QuantityOnHand = variant.Inventory != null ? variant.Inventory.QuantityOnHand : 0,
                            ReorderPoint = variant.Inventory != null ? variant.Inventory.ReorderPoint : 0,
                            IsActive = variant.IsActive
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
