using M.YZ.Basement.Utilities;
using M.YZ.Basement.Core.Contracts.Data.Commands;

namespace M.YZ.Basement.Infra.Data.Sql.Commands;
public abstract class BaseEntityFrameworkUnitOfWork<TDbContext> : IUnitOfWork
    where TDbContext : BaseCommandDbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly BasementServices _basementApplicationService;

    public BaseEntityFrameworkUnitOfWork(TDbContext dbContext, BasementServices basementApplicationContext)
    {
        _dbContext = dbContext;
        _basementApplicationService = basementApplicationContext;
    }

    public void BeginTransaction()
    {
        _dbContext.BeginTransaction();
    }

    public int Commit()
    {
        var result = _dbContext.SaveChanges();
        return result;
    }

    public async Task<int> CommitAsync()
    {
        var result = await _dbContext.SaveChangesAsync();
        return result;
    }

    public void CommitTransaction()
    {
        _dbContext.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _dbContext.RollbackTransaction();
    }
}

