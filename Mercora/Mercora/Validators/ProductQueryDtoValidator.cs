using FluentValidation;
using Mercora.Application.Dtos.Products;

namespace Mercora.Api.Validators
{
    public class ProductQueryDtoValidator : AbstractValidator<ProductQueryDto>
    {
        private static readonly HashSet<string> AllowedSorts =
        [
            "newest", "priceAsc", "priceDesc", "nameAsc", "nameDesc"
        ];

        public ProductQueryDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.Sort)
                .Must(s => string.IsNullOrWhiteSpace(s) || AllowedSorts.Contains(s.Trim()))
                .WithMessage("Invalid sort. Allowed: newest, priceAsc, priceDesc, nameAsc, nameDesc.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice is not null);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxPrice is not null);

            RuleFor(x => x)
                .Must(x => x.MinPrice is null || x.MaxPrice is null || x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice cannot be greater than MaxPrice.");
        }
    }
}
