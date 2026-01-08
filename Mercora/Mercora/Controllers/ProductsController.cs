using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercora.Infrastructure.Persistence;

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
                .Select(product => new
                    {
                        product.ProductId,
                        product.Name,
                        product.Slug,
                        product.BasePrice,
                        product.CurrencyCode
                    }).ToListAsync();

            return Ok(products);
        }

        [HttpGet("{Slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            var product = await _db.Products
                .Where(product => product.Slug == slug && product.IsPublished && !product.IsDeleted)
                .Select(product => new
                {
                    product.ProductId,
                    product.Name,
                    product.Slug,
                    product.Description,
                    product.BasePrice,
                    product.CurrencyCode
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
