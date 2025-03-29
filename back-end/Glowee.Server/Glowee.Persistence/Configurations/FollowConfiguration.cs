using Glowee.Domain.Entities.Follows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(f => f.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(f => f.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(f => f.FollowerId)
                   .IsRequired();
            builder.HasOne(f => f.Follower)
                   .WithMany(u => u.Followings)
                   .HasForeignKey(f => f.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(f => f.FollowedId)
                   .IsRequired();
            builder.HasOne(f => f.Followed)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(f => f.FollowedId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
