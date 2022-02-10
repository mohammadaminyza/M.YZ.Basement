using FluentValidation.AspNetCore;
using M.YZ.Basement.EndPoints.Web.Filters;
using M.YZ.Basement.EndPoints.Web.Middlewares.ApiExceptionHandler;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using M.YZ.Infra.Auth.ControllerDetectors.ASPServices;

namespace M.YZ.Basement.EndPoints.Web.StartupExtensions
{
    public static class AddApiConfigurationExtensions
    {
        public static IServiceCollection AddBasementApiServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var basementConfigurations = new BasementConfigurationOptions();
            configuration.GetSection(basementConfigurations.SectionName).Bind(basementConfigurations);
            services.AddSingleton(basementConfigurations);
            services.AddScoped<ValidateModelStateAttribute>();

            if (basementConfigurations.ApiArchitecturalType == ApiArchitecturalType.Rest)
            {
                services.AddControllers(options =>
                {
                    options.Filters.AddService<ValidateModelStateAttribute>();
                    options.Filters.Add(typeof(TrackActionPerformanceFilter));
                }).AddFluentValidation();

            }

            else if (basementConfigurations.ApiArchitecturalType == ApiArchitecturalType.GRpc)
            {
                services.AddGrpc(options =>
                {
                    options.Interceptors.Add<GRpcApiExceptionHandler>();
                    options.EnableDetailedErrors = true;
                });

                services.AddBasementGRpc(basementConfigurations);
            }

            services.AddEndpointsApiExplorer();

            services.AddBasementDependencies(basementConfigurations.AssmblyNameForLoad.Split(','));

            services.AddCors();
            services.AddAuthorization();

            if (basementConfigurations.ApiArchitecturalType == ApiArchitecturalType.Rest)
                AddSwagger(services);

            return services;
        }

        public static void UseBasementApiConfigure(this IApplicationBuilder app, BasementConfigurationOptions configuration, IWebHostEnvironment env)
        {
            if (configuration.ApiArchitecturalType == ApiArchitecturalType.Rest)
                app.UseApiExceptionHandler(options =>
                {
                    ApiExceptionOptions.ApiExceptionOptionDefaultSetting(options);
                });

            if (configuration.ApiArchitecturalType == ApiArchitecturalType.Rest && configuration.AppPart.Enabled)
            {
                var appPartRegistrar = app.ApplicationServices.GetRequiredService<AppPartRegistrar>();
                appPartRegistrar.Register().Wait();
            }

            app.UseStatusCodePages();
            if (configuration.ApiArchitecturalType == ApiArchitecturalType.Rest && configuration.Swagger != null && configuration.Swagger.SwaggerDoc != null)
            {

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(configuration.Swagger.SwaggerDoc.URL, configuration.Swagger.SwaggerDoc.Title);
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private static void AddSwagger(IServiceCollection services)
        {
            var basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            if (basementConfigurations?.Swagger?.Enabled == true && basementConfigurations.Swagger.SwaggerDoc != null)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(basementConfigurations.Swagger.SwaggerDoc.Name, new OpenApiInfo { Title = basementConfigurations.Swagger.SwaggerDoc.Title, Version = basementConfigurations.Swagger.SwaggerDoc.Version });
                });
            }
        }
    }
}
