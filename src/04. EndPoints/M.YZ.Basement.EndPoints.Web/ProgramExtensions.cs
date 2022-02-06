namespace M.YZ.Basement.EndPoints.Web;

public static class ProgramExtensions
{
    public static IHostBuilder DefaultHostBuilder(this IHostBuilder host, params string[] appSettingFiles)
    {
        return host
            .ConfigureAppConfiguration((ctx, config) =>
            {
                config.AddAppSettings(appSettingFiles);
            })
            .UseSerilog((ctx, lc) =>
            {
                lc.ReadFrom
                    .Configuration(ctx.Configuration);
            });
    }

    public static void AddLogger(this WebApplicationBuilder builder, string[] appSettingFiles)
    {
        builder.Configuration.AddAppSettings(appSettingFiles);
        AddLogger(builder.Configuration);
    }

    private static void AddLogger(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    private static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configurationBuilder, params string[] appSettingFiles)
    {
        foreach (var item in appSettingFiles)
        {
            configurationBuilder.AddJsonFile(item, true);
        }

        return configurationBuilder;
    }
}