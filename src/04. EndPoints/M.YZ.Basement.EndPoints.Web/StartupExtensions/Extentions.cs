using M.YZ.Basement.Infra.IoC;
using M.YZ.Basement.Utilities.DependencyInjection;

namespace M.YZ.Basement.EndPoints.Web.StartupExtensions
{
    public static class Extensions
    {

        public static IServiceCollection AddBasementDependencies(this IServiceCollection services,
            params string[] assemblyNamesForSearch)
        {

            var assemblies = GetAssemblies(assemblyNamesForSearch);
            services.AddInterfaceDependenciesLifeTime(assemblies)
                .AddApplicationServices(assemblies)
                .AddDataAccess(assemblies)
                .AddBasementServices(assemblies);
            return services;
        }

        private static List<Assembly> GetAssemblies(string[] assmblyName)
        {

            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateCompilationLibrary(library, assmblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }
        private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assmblyName)
        {
            return assmblyName.Any(d => compilationLibrary.Name.Contains(d))
                || compilationLibrary.Dependencies.Any(d => assmblyName.Any(c => d.Name.Contains(c)));
        }

    }
}