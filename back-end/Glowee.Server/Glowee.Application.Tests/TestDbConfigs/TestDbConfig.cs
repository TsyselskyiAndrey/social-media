using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Glowee.Application.Tests.TestDbConfigs;

public static class TestDbConfig<TContext> where TContext : DbContext
{
    public static TContext GetContext()
    {
        var options = GetOptions();
        
        var ctor = typeof(TContext).GetConstructor([typeof(DbContextOptions<TContext>)]);
        if (ctor == null)
            throw new InvalidOperationException(
                $"No constructor found for {typeof(TContext).Name} that takes DbContextOptions<{typeof(TContext).Name}>."
            );

        var context = (TContext)ctor.Invoke([options]);
        context.Database.EnsureCreated();

        return context;
    }

    private static DbContextOptions<TContext> GetOptions()
    {
        var builder = new DbContextOptionsBuilder<TContext>();
        builder.EnableSensitiveDataLogging();

        builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
            {
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning);
            });

        return builder.Options;
    }
}