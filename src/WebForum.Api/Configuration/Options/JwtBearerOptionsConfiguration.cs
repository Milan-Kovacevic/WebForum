using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebForum.Infrastructure.Options;

namespace WebForum.Api.Configuration.Options;

public class JwtBearerOptionsConfiguration(IOptions<JwtOptions> options) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _options = options.Value;

    public void Configure(string? name, JwtBearerOptions options) => Configure(options);

    public void Configure(JwtBearerOptions options)
    {
        IEnumerable<SymmetricSecurityKey> signingKeys =
        [
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.AccessTokenSigningKey)),
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.RefreshTokenSigningKey))
        ];

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKeys = signingKeys,
            ClockSkew = TimeSpan.Zero
        };
    }
}