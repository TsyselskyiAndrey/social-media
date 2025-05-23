using Glowee.Domain.Entities.LikedPosts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class LikedPostConfiguration : IEntityTypeConfiguration<LikedPost>
    {
        public void Configure(EntityTypeBuilder<LikedPost> builder)
        {
            builder.HasKey(lp => lp.Id);

            builder.Property(lp => lp.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(lp => lp.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(lp => lp.ModifiedAt)
                  .IsRequired()
                  .HasDefaultValueSql("getdate()");

            builder.Property(lp => lp.PostId)
                   .IsRequired();
            builder.HasOne(lp => lp.Post)
                   .WithMany(p => p.LikedPosts)
                   .HasForeignKey(lp => lp.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(lp => lp.UserId)
                   .IsRequired();
            builder.HasOne(lp => lp.User)
                   .WithMany(u => u.LikedPosts)
                   .HasForeignKey(lp => lp.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
