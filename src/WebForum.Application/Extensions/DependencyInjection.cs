using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace WebForum.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Value));
        services.AddValidatorsFromAssembly(AssemblyReference.Value);
        return services;
    }
}