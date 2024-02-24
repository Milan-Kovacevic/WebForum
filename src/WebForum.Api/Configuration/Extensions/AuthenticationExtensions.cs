using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebForum.Api.Configuration.Options;

namespace WebForum.Api.Configuration.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer()
            .AddOAuth("github", o =>
            {
                o.ClientId = "5060f298dec405ced7b8";
                o.ClientSecret = "1c0b4df3c2762671599cea0b510c7b4e79474631";
                o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                o.TokenEndpoint = "https://github.com/login/oauth/access_token";
                o.UserInformationEndpoint = "https://api.github.com/user";
                o.CallbackPath = "/api/oauth/github";
            });
        services.ConfigureOptions<JwtBearerOptionsConfiguration>();
        services.AddAuthorization();
        return services;
    }
}