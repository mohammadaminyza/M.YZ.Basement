using FluentValidation.AspNetCore;
using M.YZ.Basement.EndPoints.Web.Filters;
using M.YZ.Basement.EndPoints.Web.Middlewares.ApiExceptionHandler;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;

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
            services.AddControllers(options =>
            {
                options.Filters.AddService<ValidateModelStateAttribute>();
                options.Filters.Add(typeof(TrackActionPerformanceFilter));
            }).AddFluentValidation();

            services.AddEndpointsApiExplorer();

            services.AddBasementDependencies(basementConfigurations.AssmblyNameForLoad.Split(','));

            AddSwagger(services);
            return services;
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
        public static void UseBasementApiConfigure(this IApplicationBuilder app, BasementConfigurationOptions configuration, IWebHostEnvironment env)
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
            if (configuration.Swagger != null && configuration.Swagger.SwaggerDoc != null)
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }




    }
}
