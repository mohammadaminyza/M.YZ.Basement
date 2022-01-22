using System.Reflection;
using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.ApplicationServices.Events;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Events;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Queries;
using M.YZ.Basement.Core.Contracts.Data.Commands;
using M.YZ.Basement.Core.Contracts.Data.Queries;
using M.YZ.Basement.Utilities.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace M.YZ.Basement.Infra.IoC;

public static class DependencyContainer
{
    public static IServiceCollection AddInterfaceDependenciesLifeTime(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return services.AddWithTransientLifetime(assemblies, typeof(ITransientLifetime))
            .AddWithScopedLifetime(assemblies, typeof(IScopeLifetime))
            .AddWithSingletonLifetime(assemblies, typeof(ISingletoneLifetime));
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(ICommandRepository<>), typeof(IQueryRepository));

    public static IServiceCollection AddUnitOfWorks(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(IUnitOfWork));

    public static IServiceCollection AddWithTransientLifetime(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        params Type[] assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
            .AddClasses(c => c.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        return services;
    }

    public static IServiceCollection AddWithScopedLifetime(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        params Type[] assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
            .AddClasses(c => c.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }

    public static IServiceCollection AddWithSingletonLifetime(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        params Type[] assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
            .AddClasses(c => c.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(IQueryHandler<,>), typeof(IQueryDispatcher));

    public static IServiceCollection AddEventHandlers(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(IDomainEventHandler<>), typeof(IEventDispatcher));

    public static IServiceCollection AddFluentValidators(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddValidatorsFromAssemblies(assembliesForSearch);

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(ICommandHandler<>), typeof(ICommandHandler<,>));

    public static IServiceCollection AddCommandDispatcherDecorators(this IServiceCollection services)
    {
        services.AddTransient<CommandDispatcher, CommandDispatcher>();
        services.AddTransient<CommandDispatcherDomainExceptionHandlerDecorator, CommandDispatcherDomainExceptionHandlerDecorator>();
        services.AddTransient<CommandDispatcherValidationDecorator, CommandDispatcherValidationDecorator>();
        services.AddTransient<ICommandDispatcher, CommandDispatcherValidationDecorator>();
        return services;
    }

}