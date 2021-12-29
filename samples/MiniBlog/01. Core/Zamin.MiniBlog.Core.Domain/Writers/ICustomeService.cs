using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M.YZ.Basement.MiniBlog.Core.Domain.Writers
{
    public interface ICustomeServiceTransient
    {
        void Exec();
    }
    public interface ICustomeServiceSingletone
    {
        void Exec();
    }
    public interface ICustomeServiceScope
    {
        void Exec();
    }
}
