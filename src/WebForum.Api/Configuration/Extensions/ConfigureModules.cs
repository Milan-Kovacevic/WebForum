using WebForum.Application.Extensions;
using WebForum.Infrastructure.Extensions;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureModules
{
    public static IHostApplicationBuilder AddModules(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        return builder;
    }
}