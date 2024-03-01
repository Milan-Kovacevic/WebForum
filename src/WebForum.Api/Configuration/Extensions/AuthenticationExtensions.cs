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
            //.AddGitHub(options =>
            //{
            //    options.ClientId = "5060f298dec405ced7b8";
            //    options.ClientSecret = "1c0b4df3c2762671599cea0b510c7b4e79474631";
            //    options.Events.OnTicketReceived = async (ctx) =>
            //    {
            //        var userId = ctx.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            //        ctx.HandleResponse();
            //        ctx.Response.Redirect($"api/Test?code={userId ?? "empty"}");
            //        await Task.CompletedTask;
            //    };
            //});
        services.ConfigureOptions<JwtBearerOptionsConfiguration>();
        services.AddAuthorization();
        return services;
    }
}