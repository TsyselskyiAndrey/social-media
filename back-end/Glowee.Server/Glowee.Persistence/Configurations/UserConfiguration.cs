using Glowee.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .HasConversion(id => id.Value, value => new(value));

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
                   .HasMaxLength(511);

            builder.HasIndex(c => c.UserName, "IX_Users_Handle")
                   .IsUnique();

            builder.Property(c => c.Biography)
                   .IsRequired(false)
                   .HasMaxLength(1024);

            builder.Property(c => c.ProfileImageUrl)
                   .IsRequired(false)
                   .HasMaxLength(512);

            builder.Property(c => c.BirthDate)
                   .IsRequired(false);
        }
    }
}
