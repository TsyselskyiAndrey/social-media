using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace Glowee.Application.Tests.TestDbConfigs;

public class TestContext : IDisposable
{
    public SqlDbContext Context { get; private set; }
    private IDbContextTransaction? _transaction { get;  set; }

    public TestContext()
    {
        Context = TestDbConfig<SqlDbContext>.GetContext();
    }

    public void BeginTransaction()
    {
        _transaction = Context.Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }
    
    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}