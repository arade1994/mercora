using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercora.Infrastructure.Persistence;
using Mercora.Api.Dtos.Products;

namespace Mercora.Api.Controllers
{
    public class ProductsController(MercoraDbContext db) : ControllerBase
    {
        private readonly MercoraDbContext _db = db;

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _db.Products
                .Where(product => product.IsPublished && !product.IsDeleted)
                .OrderByDescending(product => product.CreatedAtUtc)
                .Select(product => new ProductListItemDto
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Slug = product.Slug,
                        BasePrice = product.BasePrice,
                        CurrencyCode = product.CurrencyCode
                    }).ToListAsync();

            return Ok(products);
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
