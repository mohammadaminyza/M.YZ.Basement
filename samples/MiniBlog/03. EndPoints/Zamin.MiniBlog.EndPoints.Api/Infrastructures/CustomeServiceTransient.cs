using M.YZ.Basement.Infra.IoC.DependentyInjection;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers;

namespace M.YZ.Basement.MiniBlog.EndPoints.Api.Infrastructures
{
    public class CustomeServiceTransient : ICustomeServiceTransient, ITransientLifetime
    {
        public void Exec()
        {
            
        }
    }

    public class CustomeServiceScope : ICustomeServiceScope, IScopeLifetime
    {
        public void Exec()
        {

        }
    }
    public class CustomeServiceSingletone : ICustomeServiceSingletone, ISingletoneLifetime
    {
        public void Exec()
        {

        }
    }

}
