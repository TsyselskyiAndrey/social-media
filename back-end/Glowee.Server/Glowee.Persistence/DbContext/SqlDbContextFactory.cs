using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Glowee.Persistence
{
    public class SqlDbContextFactory : IDesignTimeDbContextFactory<SqlDbContext>
    {
        public SqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlDbContext>();

            var connectionString =
                "Server=ANDREYTS_PC;Database=GloweeServer;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True;Pooling=true; Min Pool Size=5; Max Pool Size=100;";

            optionsBuilder.UseSqlServer(connectionString);

            return new SqlDbContext(optionsBuilder.Options);
        }
    }
}