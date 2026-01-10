using FluentValidation;
using Mercora.Application.Dtos.Orders;

namespace Mercora.Api.Validators
{
    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequestDto>
    {
        public PlaceOrderRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.Lines)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.Lines).ChildRules(line =>
            {
                line.RuleFor(l => l.VariantId).GreaterThan(0);
                line.RuleFor(l => l.Quantity).GreaterThan(0);
            });

            RuleFor(x => x.Lines)
                .Must(lines => lines is null || lines.Select(lines => lines.VariantId).Distinct().Count() == lines.Count())
                .WithMessage("Duplicate VariantId lines are not allowed");
        }
    }
}
