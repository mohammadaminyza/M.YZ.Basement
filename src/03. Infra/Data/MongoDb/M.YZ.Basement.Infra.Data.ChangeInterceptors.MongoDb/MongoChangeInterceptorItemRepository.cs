using System.Collections.Generic;
using System.Threading.Tasks;
using M.YZ.Basement.Infra.Data.ChangeInterceptors.EntityChageInterceptorItems;

namespace M.YZ.Basement.Infra.Data.ChangeInterceptors.MongoDb
{
    public class MongoEntityChangeInterceptorItemRepository : IEntityChageInterceptorItemRepository
    {
        public void Save(List<EntityChageInterceptorItem> entityChageInterceptorItems)
        {
            
        }

        public Task SaveAsync(List<EntityChageInterceptorItem> entityChageInterceptorItems)
        {
            throw new System.NotImplementedException();
        }
    }
}