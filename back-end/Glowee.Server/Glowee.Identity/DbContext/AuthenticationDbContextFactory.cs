using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Glowee.Identity.DbContext;

public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-LCBASFT;Database=GloweeServer;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Pooling=true;Min Pool Size=5;Max Pool Size=100;");
        
        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}
