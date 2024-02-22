using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebForum.Infrastructure.Settings;

namespace WebForum.Api.Configuration.Options;

public class JwtBearerOptionsConfiguration(IOptions<JwtOptions> options) : IConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _options = options.Value;
    
    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    }
}