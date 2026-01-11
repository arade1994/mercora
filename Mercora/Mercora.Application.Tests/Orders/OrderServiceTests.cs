using FluentAssertions;
using Mercora.Application.Abstractions.Persistence;
using Mercora.Application.Common.Exceptions;
using Mercora.Application.Dtos.Orders;
using Mercora.Infrastructure.Services.Orders;
using Moq;

namespace Mercora.Application.Tests.Orders
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repo = new(MockBehavior.Strict);
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(_repo.Object);
        }

        [Fact]
        public async Task PlaceOrderAsync_returns_response_from_repository()
        {
            var request = new PlaceOrderRequestDto
            {
                UserId = 1,
                Lines =
                [
                    new PlaceOrderLineDto { VariantId = 101, Quantity = 2 }
                ]
            };

            var expected = new PlaceOrderResponseDto
            {
                OrderId = 55,
                OrderNumber = "MERC-2026-000055",
                TotalAmount = 500,
                CurrencyCode = "EUR"
            }; 

            _repo.Setup(r => r.PlaceOrderAsync(request))
                 .ReturnsAsync(expected);

            var result = await _service.PlaceOrderAsync(request);

            result.Should().BeEquivalentTo(expected);
            _repo.Verify(r => r.PlaceOrderAsync(request), Times.Once);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PlaceOrderAsync_throws_business_rule_exception_if_repo_throws()
        {
            var request = new PlaceOrderRequestDto
            {
                UserId = 1,
                Lines = [new PlaceOrderLineDto { VariantId = 101, Quantity = 999 }]
            };

            _repo.Setup(r => r.PlaceOrderAsync(request))
                 .ThrowsAsync(new BusinessRuleException(50005, "Insufficient stock"));

            var act = () => _service.PlaceOrderAsync(request);

            await act.Should().ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == 50005);

            _repo.Verify(r => r.PlaceOrderAsync(request), Times.Once);
            _repo.VerifyNoOtherCalls();
        }
    }
}
