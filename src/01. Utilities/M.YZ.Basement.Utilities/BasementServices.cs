using M.YZ.Basement.Utilities.Services.Chaching;
using M.YZ.Basement.Utilities.Services.Localizations;
using M.YZ.Basement.Utilities.Services.ObjectMappers;
using M.YZ.Basement.Utilities.Services.Serializers;
using M.YZ.Basement.Utilities.Services.Users;
using Microsoft.Extensions.Logging;

namespace M.YZ.Basement.Utilities;

public class BasementServices
{
    public readonly ITranslator Translator;
    public readonly ICacheAdapter CacheAdapter;
    public readonly IMapperAdapter MapperFacade;
    public readonly ILoggerFactory LoggerFactory;
    public readonly IJsonSerializer Serializer;
    public readonly IUserInfoService UserInfoService;

    public BasementServices(ITranslator translator,
            ILoggerFactory loggerFactory,
            IJsonSerializer serializer,
            IUserInfoService userInfoService,
            ICacheAdapter cacheAdapter,
            IMapperAdapter mapperFacade)
    {
        Translator = translator;
        LoggerFactory = loggerFactory;
        Serializer = serializer;
        UserInfoService = userInfoService;
        CacheAdapter = cacheAdapter;
        MapperFacade = mapperFacade;
    }
}

