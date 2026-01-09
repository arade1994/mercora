using Microsoft.AspNetCore.Mvc;
using Mercora.Application.Dtos.Products;
using Mercora.Application.Dtos.Common;
using Mercora.Application.Products;

namespace Mercora.Api.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _products;

        public ProductsController(IProductService products) => _products = products;

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<PagedResultDto<ProductListItemDto>>> GetProducts([FromQuery] ProductQueryDto q) => Ok(await _products.GetProductsAsync(q));

        [HttpGet("api/[controller]/{Slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            var product = await _products.GetProductBySlugAsync(slug);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
