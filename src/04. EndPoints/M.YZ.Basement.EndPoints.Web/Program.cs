namespace M.YZ.Basement.EndPoints.Web;

public class BasementProgram
{
    public WebApplicationBuilder Main(string[] args, params string[] appSettingFiles)
    {
        var builder = CreateHostBuilder(args, appSettingFiles);

        builder.AddLogger(appSettingFiles);

        try
        {
            StartLog();

            return builder;
        }
        catch (Exception ex)
        {
            FatalLog(ex);

            return builder;
        }
        finally
        {
            CloseAndFlushLog();
        }
    }

    public int Main(string[] args, Type startUp, params string[] appSettingFiles)
    {
        try
        {
            var builder =
                CreateHostBuilder(args, startUp, appSettingFiles);

            StartLog();

            builder.Build().Run();

            return 0;
        }
        catch (Exception ex)
        {
            FatalLog(ex);

            return 1;
        }
        finally
        {
            CloseAndFlushLog();
        }
    }

    private WebApplicationBuilder CreateHostBuilder(string[] args, params string[] appSettingFiles)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host
            .DefaultHostBuilder(appSettingFiles);

        return builder;
    }

    private IHostBuilder CreateHostBuilder(string[] args, Type startUp, params string[] appSettingFiles)
    {
        return Host.CreateDefaultBuilder(args)
            .DefaultHostBuilder(appSettingFiles)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup(startUp);
            });
    }

    private void StartLog()
    {
        Log.Information("Starting web host");
    }
    private void FatalLog(Exception ex)
    {
        Log.Fatal(ex, "Host terminated unexpectedly");
    }
    private void CloseAndFlushLog()
    {
        Log.CloseAndFlush();
    }
}