using Serilog;

namespace WebForum.Api.Configuration.Extensions;

public static class ConfigureLogging
{
    public static IHostBuilder AddLogging(this IHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration);
        });
        return host;
    }
}