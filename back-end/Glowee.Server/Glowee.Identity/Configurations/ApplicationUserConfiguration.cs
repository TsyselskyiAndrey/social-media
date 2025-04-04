using Glowee.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Identity.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> builder)
        {
            var hasher = new PasswordHasher<AuthUser>();
            builder.HasData(
                 new AuthUser
                 {
                     Id = -1,
                     Email = "tsyselskyiandrey@gmail.com",
                     NormalizedEmail = "TSYSELSKYIANDREY@GMAIL.COM",
                     UserName = "AdminProMax",
                     NormalizedUserName = "ADMINPROMAX",
                     PasswordHash = hasher.HashPassword(null, "Q1w2e3r4t5y6"),
                     EmailConfirmed = true
                 }
            );


            builder.HasKey(u => u.Id);

            builder.Property(u => u.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(r => r.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(511);

            builder.Property(c => c.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.BannedUntil)
                   .IsRequired()
                   .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

            builder.Property(c => c.EmailConfirmationCode)
                   .IsRequired(false)
                   .HasMaxLength(15);

            builder.Property(u => u.EmailConfirmationCodeExpiryTime)
                   .IsRequired()
                   .HasDefaultValueSql("'0001-01-01T00:00:00.000'");
        }
    }
}
