using Glowee.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Identity.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                 new ApplicationUser
                 {
                     Id = -1,
                     Email = "tsyselskyiandrey@gmail.com",
                     NormalizedEmail = "TSYSELSKYIANDREY@GMAIL.COM",
                     FirstName = "System",
                     LastName = "Admin",
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

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(511);

            builder.Property(c => c.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.Biography)
                   .IsRequired(false)
                   .HasMaxLength(1024);

            builder.Property(u => u.BannedUntil)
                   .IsRequired()
                   .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

            builder.Property(c => c.ProfileImageUrl)
                   .IsRequired(false)
                   .HasMaxLength(512);

            builder.Property(c => c.BirthDate)
                   .IsRequired(false);

            builder.Property(c => c.EmailConfirmationCode)
                   .IsRequired(false)
                   .HasMaxLength(15);

            builder.Property(u => u.EmailConfirmationCodeExpiryTime)
                   .IsRequired()
                   .HasDefaultValueSql("'0001-01-01T00:00:00.000'");
        }
    }
}
