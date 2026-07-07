using FluentValidation;
using Domain.Models;

namespace Domain.Validators;

public sealed class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(p => p.Price)
            .GreaterThan(0);
    }
}