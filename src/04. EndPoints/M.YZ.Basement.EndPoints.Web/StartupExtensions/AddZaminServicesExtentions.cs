using M.YZ.Basement.Infra.Data.ChangeInterceptors.EntityChageInterceptorItems;
using M.YZ.Basement.Infra.Events.Outbox;
using M.YZ.Basement.Infra.Events.PoolingPublisher;
using M.YZ.Basement.Infra.Tools.Caching.Microsoft;
using M.YZ.Basement.Infra.Tools.OM.AutoMapper.DipendencyInjections;
using M.YZ.Basement.Messaging.IdempotentConsumers;
using M.YZ.Basement.Utilities;
using M.YZ.Basement.Utilities.Services.Chaching;
using M.YZ.Basement.Utilities.Services.Localizations;
using M.YZ.Basement.Utilities.Services.Logger;
using M.YZ.Basement.Utilities.Services.MessageBus;
using M.YZ.Basement.Utilities.Services.Serializers;
using M.YZ.Basement.Utilities.Services.Users;
using M.YZ.Infra.Auth.ControllerDetectors.ASPServices;
using M.YZ.Infra.Auth.ControllerDetectors.Data;
using Microsoft.AspNetCore.Http;

namespace M.YZ.Basement.EndPoints.Web.StartupExtensions
{
    public static class AddBasementServicesExtensions
    {
        public static IServiceCollection AddBasementServices(
            this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            services.AddCaching();
            services.AddSession();
            services.AddLogging();
            services.AddJsonSerializer(assembliesForSearch);
            services.AddExcelSerializer(assembliesForSearch);
            services.AddObjectMapper(assembliesForSearch);
            services.AddUserInfoService(assembliesForSearch);
            services.AddTranslator(assembliesForSearch);
            services.AddMessageBus(assembliesForSearch);
            services.AddPoolingPublisher(assembliesForSearch);
            services.AddTransient<BasementServices>();
            services.AddEntityChangeInterception(assembliesForSearch);
            services.AddControllerDetectors(assembliesForSearch);
            return services;
        }

        private static IServiceCollection AddCaching(this IServiceCollection services)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            if (_basementConfigurations?.Caching?.Enable == true)
            {
                if (_basementConfigurations.Caching.Provider == CacheProvider.MemoryCache)
                {
                    services.AddTransient<ICacheAdapter, InMemoryCacheAdapter>();
                }
                else
                {
                    services.AddTransient<ICacheAdapter, DistributedCacheAdapter>();
                }

                switch (_basementConfigurations.Caching.Provider)
                {
                    case CacheProvider.DistributedSqlServerCache:
                        services.AddDistributedSqlServerCache(options =>
                        {
                            options.ConnectionString = _basementConfigurations.Caching.DistributedSqlServerCache.ConnectionString;
                            options.SchemaName = _basementConfigurations.Caching.DistributedSqlServerCache.SchemaName;
                            options.TableName = _basementConfigurations.Caching.DistributedSqlServerCache.TableName;
                        });
                        break;
                    case CacheProvider.StackExchangeRedisCache:
                        services.AddStackExchangeRedisCache(options =>
                        {
                            options.Configuration = _basementConfigurations.Caching.StackExchangeRedisCache.Configuration;
                            options.InstanceName = _basementConfigurations.Caching.StackExchangeRedisCache.SampleInstance;
                        });
                        break;
                    case CacheProvider.NCacheDistributedCache:
                        throw new NotSupportedException("NCache Not Supporting yet");
                    default:
                        services.AddMemoryCache();
                        break;
                }
            }
            else
            {
                services.AddScoped<ICacheAdapter, FakeCacheAdapter>();
            }
            return services;
        }
        private static IServiceCollection AddSession(this IServiceCollection services)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            if (_basementConfigurations?.Session?.Enable == true)
            {
                var eveSessionCookie = _basementConfigurations.Session.Cookie;
                CookieBuilder cookieBuilder = new();
                cookieBuilder.Name = eveSessionCookie.Name;
                cookieBuilder.Domain = eveSessionCookie.Domain;
                cookieBuilder.Expiration = eveSessionCookie.Expiration;
                cookieBuilder.HttpOnly = eveSessionCookie.HttpOnly;
                cookieBuilder.IsEssential = eveSessionCookie.IsEssential;
                cookieBuilder.MaxAge = eveSessionCookie.MaxAge;
                cookieBuilder.Path = eveSessionCookie.Path;
                cookieBuilder.SameSite = Enum.Parse<SameSiteMode>(eveSessionCookie.SameSite.ToString());
                cookieBuilder.SecurePolicy = Enum.Parse<CookieSecurePolicy>(eveSessionCookie.SecurePolicy.ToString());

                services.AddSession(options =>
                {
                    options.Cookie = cookieBuilder;
                    options.IdleTimeout = _basementConfigurations.Session.IdleTimeout;
                    options.IOTimeout = _basementConfigurations.Session.IOTimeout;
                });
            }
            return services;
        }
        private static IServiceCollection AddLogging(this IServiceCollection services)
        {
            return services.AddScoped<IScopeInformation, ScopeInformation>();
        }

        public static IServiceCollection AddJsonSerializer(this IServiceCollection services, IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetRequiredService<BasementConfigurationOptions>();
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(c => c.Where(type => type.Name == _basementConfigurations.JsonSerializerTypeName && typeof(IJsonSerializer).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            return services;
        }

        public static IServiceCollection AddExcelSerializer(this IServiceCollection services, IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetRequiredService<BasementConfigurationOptions>();
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.ExcelSerializerTypeName && typeof(IExcelSerializer).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            return services;
        }

        private static IServiceCollection AddObjectMapper(this IServiceCollection services, IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            if (_basementConfigurations.RegisterAutomapperProfiles)
            {
                services.AddAutoMapperProfiles(assembliesForSearch);
            }
            return services;
        }
        private static IServiceCollection AddUserInfoService(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.UserInfoServiceTypeName && typeof(IUserInfoService).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return services;
        }
        private static IServiceCollection AddTranslator(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.Translator.TranslatorTypeName && typeof(ITranslator).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            return services;
        }


        private static IServiceCollection AddMessageBus(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();

            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.MessageBus.MessageConsumerTypeName && typeof(IMessageConsumer).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.Scan(s => s.FromAssemblies(assembliesForSearch)
             .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.Messageconsumer.MessageInboxStoreTypeName && typeof(IMessageInboxItemRepository).IsAssignableFrom(type)))
             .AsImplementedInterfaces()
             .WithSingletonLifetime());

            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(classes => classes.Where(type => type.Name.StartsWith(_basementConfigurations.MessageBus.MessageBusTypeName) && typeof(ISendMessageBus).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            services.Scan(s => s.FromAssemblies(assembliesForSearch)
                .AddClasses(classes => classes.Where(type => type.Name.StartsWith(_basementConfigurations.MessageBus.MessageBusTypeName) && typeof(IReceiveMessageBus).IsAssignableFrom(type)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());


            services.AddHostedService<IdempotentConsumerHostedService>();
            return services;
        }

        private static IServiceCollection AddPoolingPublisher(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            if (_basementConfigurations.PoolingPublisher.Enabled)
            {
                services.Scan(s => s.FromAssemblies(assembliesForSearch)
                    .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.PoolingPublisher.OutBoxRepositoryTypeName && typeof(IOutBoxEventItemRepository).IsAssignableFrom(type)))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());
                services.AddHostedService<PoolingPublisherHostedService>();

            }
            return services;
        }

        private static IServiceCollection AddEntityChangeInterception(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            var _basementConfigurations = services.BuildServiceProvider().GetService<BasementConfigurationOptions>();
            if (_basementConfigurations.EntityChangeInterception.Enabled)
            {
                services.Scan(s => s.FromAssemblies(assembliesForSearch)
                    .AddClasses(classes => classes.Where(type => type.Name == _basementConfigurations.EntityChangeInterception.
                        EntityChageInterceptorRepositoryTypeName && typeof(IEntityChageInterceptorItemRepository).IsAssignableFrom(type)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            }
            return services;
        }

        private static IServiceCollection AddControllerDetectors(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch)
        {
            var _zaminConfigurations = services.BuildServiceProvider().GetRequiredService<BasementConfigurationOptions>();
            if (_zaminConfigurations.AppPart.Enabled)
            {
                services.AddTransient<ApplicationPartDetector>();
                services.AddTransient<AppPartRegistrar>();
                services.AddTransient<ControllerDetectorRepository>();
            }
            return services;
        }
    }
}
