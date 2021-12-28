using AutoMapper;
using M.YZ.Basement.Infra.Tools.OM.AutoMapper.Common;
using M.YZ.Basement.Utilities.Services.ObjectMappers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace M.YZ.Basement.Infra.Tools.OM.AutoMapper.DipendencyInjections;
public static class AutomapperRegistration
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var profileTypes = assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(type => typeof(Profile).IsAssignableFrom(type)).ToList();
        var profiles = new List<Profile>();
        foreach (var profileType in profileTypes)
        {
            if (Activator.CreateInstance(profileType) is Profile profile)
                profiles.Add(profile);
        }
        return services.AddSingleton<IMapperAdapter>(new AutoMapperAdapter(profiles.ToArray()));
    }
}
