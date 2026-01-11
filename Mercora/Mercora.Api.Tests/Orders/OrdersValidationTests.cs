using FluentAssertions;
using Mercora.Application.Dtos.Orders;
using Mercora.Application.Orders;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Mercora.Api.Tests.Orders
{
    public class OrdersValidationTests : IClassFixture<MercoraWebApplicationFactory>
    {
        private readonly MercoraWebApplicationFactory _factory;

        public OrdersValidationTests(MercoraWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task PlaceOrder_returns_400_problem_details_when_invalid()
        {
            var mock = new Mock<IOrderService>();

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped(_ => mock.Object);
                });
            }).CreateClient();

            var request = new PlaceOrderRequestDto
            {
                UserId = 0,
                Lines = []
            };

            var res = await client.PostAsJsonAsync("/api/orders", request);

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var text = await res.Content.ReadAsStringAsync();
            text.Should().Contain("Validation failed");

            mock.Verify(s => s.PlaceOrderAsync(It.IsAny<PlaceOrderRequestDto>()), Times.Never);
        }
    }
}
