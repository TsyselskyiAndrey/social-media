using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
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

            builder.Property(c => c.Handle)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(c => c.Handle, "IX_Users_Handle")
                   .IsUnique();

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
