using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Glowee.Identity.DbContext;

public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
        optionsBuilder.UseSqlServer("Server=tcp:glowee-server.database.windows.net,1433;Initial Catalog=glowee-server-db;Persist Security Info=False;User ID=glowee-admin;Password=Q1w2e3r4t5y6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}
