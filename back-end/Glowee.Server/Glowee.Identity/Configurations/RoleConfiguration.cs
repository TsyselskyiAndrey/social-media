using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<long>> builder)
        {
            builder.HasData(
                new IdentityRole<long>
                {
                    Id = -1,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<long>
                {
                    Id = -2,
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                }
            );
        }
    }
}
