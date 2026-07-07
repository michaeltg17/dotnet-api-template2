using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Domain.Models;
using Domain.Validators;
using Core.Testing.Builders;
using Xunit;

namespace UnitTests.Domain.Validators;

public sealed class ProductValidatorTests
{
    readonly ProductValidator validator = new();

    public static TheoryData<Product, Expression<Func<Product, object>>, bool> GetTestCases()
    {
        return new TheoryData<Product, Expression<Func<Product, object>>, bool>
        {
            // Name
            // Invalid: null
            { new ProductBuilder().WithValues(p => p.Name = null!).Build(), p => p.Name, false },
            // Invalid: whitespace
            { new ProductBuilder().WithValues(p => p.Name = "      ").Build(), p => p.Name, false },
            // Invalid: empty
            { new ProductBuilder().WithValues(p => p.Name = "").Build(), p => p.Name, false },
            // Invalid: exceeds 200
            { new ProductBuilder().WithValues(p => p.Name = new string('x', 201)).Build(), p => p.Name, false },
            // Valid: max 200
            { new ProductBuilder().WithValues(p => p.Name = new string('x', 200)).Build(), p => p.Name, true },

            // Description
            // Invalid: null
            { new ProductBuilder().WithValues(p => p.Description = null!).Build(), p => p.Description, false },
            // Invalid: whitespace
            { new ProductBuilder().WithValues(p => p.Description = "      ").Build(), p => p.Description, false },
            // Invalid: empty
            { new ProductBuilder().WithValues(p => p.Description = "").Build(), p => p.Description, false },
            // Invalid: exceeds 2000
            { new ProductBuilder().WithValues(p => p.Description = new string('x', 2001)).Build(), p => p.Description, false },
            // Valid: max 2000
            { new ProductBuilder().WithValues(p => p.Description = new string('x', 2000)).Build(), p => p.Description, true },

            // Price
            // Invalid: zero
            { new ProductBuilder().WithValues(p => p.Price = 0m).Build(), p => p.Price, false },
            // Invalid: negative
            { new ProductBuilder().WithValues(p => p.Price = -5m).Build(), p => p.Price, false },
            // Valid: positive
            { new ProductBuilder().WithValues(p => p.Price = 10m).Build(), p => p.Price, true },
        };
    }

    [Theory]
    [MemberData(nameof(GetTestCases))]
    public void Validate_ShouldHaveExpectedResult(Product product, Expression<Func<Product, object>> property, bool isValid)
    {
        //When
        var result = validator.TestValidate(product);

        //Then
        if (isValid) result.ShouldNotHaveValidationErrorFor(property);
        else result.ShouldHaveValidationErrorFor(property);
    }
}