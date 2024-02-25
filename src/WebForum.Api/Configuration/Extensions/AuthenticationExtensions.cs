using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
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
        return services;
    }
    
    // Test for OAuth
    private static AuthenticationBuilder AddOAuthTest(this AuthenticationBuilder auth)
    {
        auth.AddOAuth("github", o =>
        {
            o.ClientId = "5060f298dec405ced7b8";
            o.ClientSecret = "1c0b4df3c2762671599cea0b510c7b4e79474631";
            o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            o.TokenEndpoint = "https://github.com/login/oauth/access_token";
            o.UserInformationEndpoint = "https://api.github.com/user";
            o.CallbackPath = "/signin-github";
            o.SaveTokens = false;
            o.ClaimActions.MapJsonKey("sub", "id");
            o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");

            o.Events.OnCreatingTicket = async context =>
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                using var result = await context.Backchannel.SendAsync(request);
                var user = await result.Content.ReadFromJsonAsync<JsonElement>();
                context.RunClaimActions(user);
            };
            o.Events.OnTicketReceived = context =>
            {
                var identity = context.Principal?.Identities.FirstOrDefault(identity =>
                    identity.AuthenticationType == "github");
                if (identity is null)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }

                context.HttpContext.SignInAsync(context.Principal);
                context.Success();
                return Task.CompletedTask;
            };
        });
        return auth;
    }
}