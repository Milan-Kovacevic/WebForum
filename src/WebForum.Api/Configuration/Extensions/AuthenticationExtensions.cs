using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebForum.Api.Configuration.Options;

namespace WebForum.Api.Configuration.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddRateLimiting();
        services.AddGlobalExceptionHandler();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.ConfigureOptions<JwtBearerOptionsConfiguration>();
        services.AddAuthorization();
        services.AddCors(options =>
        {
            options.AddPolicy(Constants.Cors.AllowAllPolicyName ,policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithMethods("OPTIONS");
                policy.WithOrigins("http://localhost:3000");
            });
        });
        return services;
    }
}