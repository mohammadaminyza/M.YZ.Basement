using M.YZ.Basement.Infra.IoC;

namespace M.YZ.Basement.EndPoints.Web.StartupExtensions
{
    public static class AddApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch) =>
            services
                .AddCommandHandlers(assembliesForSearch)
                .AddCommandDispatcherDecorators()
                .AddQueryHandlers(assembliesForSearch)
                .AddEventHandlers(assembliesForSearch)
                .AddFluentValidators(assembliesForSearch);
    }
}
