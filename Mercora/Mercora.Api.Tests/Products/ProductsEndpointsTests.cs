using FluentAssertions;
using Mercora.Application.Dtos.Common;
using Mercora.Application.Dtos.Products;
using Mercora.Application.Products;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Mercora.Api.Tests.Products
{
    public class ProductsEndpointsTests : IClassFixture<MercoraWebApplicationFactory>
    {
        private readonly MercoraWebApplicationFactory _factory;

        public ProductsEndpointsTests(MercoraWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task GetProducts_returns_200_and_paged_result()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(s => s.GetProductsAsync(It.IsAny<ProductQueryDto>()))
                .ReturnsAsync(new PagedResultDto<ProductListItemDto>
                {
                    Page = 1,
                    PageSize = 12,
                    TotalCount = 0,
                    Items = []
                });

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    object value = services.AddScoped(_ => mock.Object);
                });
            }).CreateClient();

            var res = await client.GetAsync("/api/products?page=1&pageSize=12");

            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await res.Content.ReadFromJsonAsync<PagedResultDto<ProductListItemDto>>();
            body.Should().NotBeNull();
            body!.Page.Should().Be(1);

            mock.Verify(s => s.GetProductsAsync(It.IsAny<ProductQueryDto>()), Times.Once);
        }

        [Fact]
        public async Task GetBySlug_returns_404_when_service_returns_null()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(s => s.GetProductBySlugAsync("unknown"))
                .ReturnsAsync((ProductDetailsDto?)null);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped(_ => mock.Object);
                });
            }).CreateClient();

            var res = await client.GetAsync("/api/products/unknown");
            
            res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
