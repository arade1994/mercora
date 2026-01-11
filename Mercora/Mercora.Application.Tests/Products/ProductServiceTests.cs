using FluentAssertions;
using Mercora.Application.Abstractions.Persistence;
using Mercora.Application.Dtos.Common;
using Mercora.Application.Dtos.Products;
using Mercora.Infrastructure.Services.Products;
using Moq;

namespace Mercora.Application.Tests.Products
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repo = new(MockBehavior.Strict);
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _service = new ProductService(_repo.Object);
        }

        [Fact]
        public async Task GetProductAsync_returns_result_from_repository()
        {
            var query = new ProductQueryDto { Page = 1, PageSize = 10, Sort = "newest" };

            var expected = new PagedResultDto<ProductListItemDto>
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 1,
                Items =
                [
                    new ProductListItemDto
                {
                    ProductId = 1,
                    Name = "Test",
                    Slug = "test",
                    BasePrice = 100,
                    CurrencyCode = "EUR"
                }
                ]
            };

            _repo.Setup(r => r.GetProductsAsync(query))
                 .ReturnsAsync(expected);

            var result = await _service.GetProductsAsync(query);

            result.Should().BeEquivalentTo(expected);
            _repo.Verify(r => r.GetProductsAsync(query), Times.Once);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductBySlugAsync_returns_product_when_found()
        {
            var slug = "standing-desk-pro";

            var expected = new ProductDetailsDto
            {
                ProductId = 10,
                Name = "Standing Desk Pro",
                Slug = slug,
                BasePrice = 250,
                CurrencyCode = "EUR",
                Images = [],
                Variants = []
            };

            _repo.Setup(r => r.GetBySlugAsync(slug))
                 .ReturnsAsync(expected);

            var result = await _service.GetProductBySlugAsync(slug);

            result.Should().BeEquivalentTo(expected);
            _repo.Verify(r => r.GetBySlugAsync(slug), Times.Once);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductBySlugAsync_returns_null_when_not_found()
        {
            var slug = "does-not-exist";

            _repo.Setup(r => r.GetBySlugAsync(slug))
                 .ReturnsAsync((ProductDetailsDto?)null);

            var result = await _service.GetProductBySlugAsync(slug);

            result.Should().BeNull();
            _repo.Verify(r => r.GetBySlugAsync(slug), Times.Once);
            _repo.VerifyNoOtherCalls();
        }
    }
}
