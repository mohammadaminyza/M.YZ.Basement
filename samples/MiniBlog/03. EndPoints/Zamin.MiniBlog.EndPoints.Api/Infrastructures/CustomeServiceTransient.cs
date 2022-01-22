﻿using M.YZ.Basement.MiniBlog.Core.Domain.Writers;
using M.YZ.Basement.Utilities.DependencyInjection;

namespace Zamin.MiniBlog.EndPoints.Api.Infrastructures
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
