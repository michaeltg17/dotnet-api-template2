using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using Domain.Validators;

namespace Application
{
    public static class DependencyConfigurator
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProductValidator>();
            services.AddScoped<ProductService>();

            return services;
        }
    }
}
