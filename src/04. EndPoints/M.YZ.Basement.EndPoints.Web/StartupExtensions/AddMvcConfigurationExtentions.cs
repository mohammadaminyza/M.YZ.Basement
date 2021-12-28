using FluentValidation.AspNetCore;
using M.YZ.Basement.EndPoints.Web.Filters;
using M.YZ.Basement.EndPoints.Web.Middlewares.ApiExceptionHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Data.SqlClient;

namespace M.YZ.Basement.EndPoints.Web.StartupExtensions
{
    public static class AddMvcConfigurationExtensions
    {
        public static IServiceCollection BasementMvcServices(this IServiceCollection services,
            IConfiguration configuration, Action<MvcOptions> mvcOptions = null)
        {
            var basementConfigurations = new BasementConfigurationOptions();
            configuration.GetSection(basementConfigurations.SectionName).Bind(basementConfigurations);
            services.AddSingleton(basementConfigurations);
            services.AddControllersWithViews(mvcOptions == null ? (options =>
            {
                options.Filters.Add(typeof(TrackActionPerformanceFilter));
            }) : mvcOptions).AddRazorRuntimeCompilation()
            .AddFluentValidation();

            if (basementConfigurations?.Session?.Enable == true)
                services.AddSession();

            services.AddBasementDependencies(basementConfigurations.AssmblyNameForLoad.Split(','));

            return services;
        }

        public static void UseBasementMvcConfigure(this IApplicationBuilder app, Action<IEndpointRouteBuilder> configur, BasementConfigurationOptions configuration, IWebHostEnvironment env)
        {
            app.UseApiExceptionHandler(options =>
            {
                options.AddResponseDetails = (context, ex, error) =>
                {
                    if (ex.GetType().Name == typeof(SqlException).Name)
                    {
                        error.Detail = "Exception was a database exception!";
                    }
                };
                options.DetermineLogLevel = ex =>
                {
                    if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                        ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LogLevel.Critical;
                    }
                    return LogLevel.Error;
                };
            });

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            if (configuration?.Session?.Enable == true)
                app.UseSession();

            app.UseEndpoints(configur);
        }
    }
}
