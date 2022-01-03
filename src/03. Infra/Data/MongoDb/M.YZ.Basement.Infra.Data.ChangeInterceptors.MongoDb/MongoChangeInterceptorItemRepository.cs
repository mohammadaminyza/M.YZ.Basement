using System.Collections.Generic;
using System.Threading.Tasks;
using M.YZ.Basement.Infra.Data.ChangeInterceptors.EntityChageInterceptorItems;

namespace M.YZ.Basement.Infra.Data.ChangeInterceptors.MongoDb
{
    public class MongoEntityChangeInterceptorItemRepository : IEntityChageInterceptorItemRepository
    {
        public void Save(List<EntityChageInterceptorItem> entityChangeInterceptorItems)
        {
            
        }

        public Task SaveAsync(List<EntityChageInterceptorItem> entityChangeInterceptorItems)
        {
            throw new System.NotImplementedException();
        }
    }
}