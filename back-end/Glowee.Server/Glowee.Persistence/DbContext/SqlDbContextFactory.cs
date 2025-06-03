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
                "Server=tcp:glowee-server.database.windows.net,1433;Initial Catalog=glowee-server-db;Persist Security Info=False;User ID=glowee-admin;Password=Q1w2e3r4t5y6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            optionsBuilder.UseSqlServer(connectionString);

            return new SqlDbContext(optionsBuilder.Options);
        }
    }
}