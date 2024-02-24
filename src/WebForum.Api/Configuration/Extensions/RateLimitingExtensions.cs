using System.Threading.RateLimiting;

namespace WebForum.Api.Configuration.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy(Constants.RateLimiter.PolicyName, context =>
            RateLimitPartition.GetFixedWindowLimiter(partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                factory: _ =>
                new FixedWindowRateLimiterOptions()
                {
                    Window = Constants.RateLimiter.Window,
                    PermitLimit = Constants.RateLimiter.PermitLimit
                }));
        });
        return services;
    }
}