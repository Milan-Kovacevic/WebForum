using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebForum.Api.Configuration.Options;

namespace WebForum.Api.Configuration.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRateLimiting();
        services.AddGlobalExceptionHandler();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.ConfigureOptions<JwtBearerOptionsConfiguration>();
        services.AddAuthorization();
        services.AddCors(options =>
        {
            options.AddPolicy(Constants.Cors.AllowWebClientPolicyName, policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins(configuration.GetSection(Constants.Cors.ClientConfigurationSection).Value!);
            });
        });
        return services;
    }
}