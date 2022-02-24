using FluentValidation;
using M.YZ.Basement.EndPoints.Web.Controllers;

namespace M.YZ.Basement.EndPoints.Web.StartupExtensions;

public static class GRpcExtension
{
    public static void AddBasementGRpc(this IServiceCollection services,
        BasementConfigurationOptions basementConfigurations)
    {
        services.AddGRpcFluentValidation();

        if (basementConfigurations.ApiArchitecturalType == ApiArchitecturalType.GRpc)
        {
            services.AddTransient<BaseGRpcServiceController>();
        }
    }

    private static IServiceCollection AddGRpcFluentValidation(this IServiceCollection service)
    {
        var validations = Assembly.GetAssembly(typeof(AbstractValidator<>));
        service.AddValidatorsFromAssembly(validations);
        return service;
    }
}