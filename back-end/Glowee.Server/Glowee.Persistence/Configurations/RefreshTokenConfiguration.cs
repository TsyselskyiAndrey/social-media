using Glowee.Domain.Common;
using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(rt => rt.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(rt => rt.UserId)
                   .IsRequired();
            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(rt => rt.DeviceId)
                   .IsRequired()
                   .HasMaxLength(2047);

            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(2047);

            builder.Property(rt => rt.TokenExpiryTime)
                   .IsRequired()
                   .HasDefaultValueSql("'0001-01-01T00:00:00.000'");
        }
    }
}
