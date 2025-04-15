using Glowee.Domain.Entities.UninterestingPosts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class UninterestingPostConfiguration : IEntityTypeConfiguration<UninterestingPost>
    {
        public void Configure(EntityTypeBuilder<UninterestingPost> builder)
        {
            builder.HasKey(up => up.Id);

            builder.Property(up => up.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(up => up.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(up => up.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(up => up.PostId)
                   .IsRequired();
            builder.HasOne(up => up.Post)
                   .WithMany(p => p.UninterestingPosts)
                   .HasForeignKey(up => up.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(up => up.UserId)
                   .IsRequired();
            builder.HasOne(up => up.User)
                   .WithMany(u => u.UninterestingPosts)
                   .HasForeignKey(up => up.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(up => up.CauseId)
                   .IsRequired();
            builder.HasOne(up => up.Cause)
                   .WithMany(c => c.UninterestingPosts)
                   .HasForeignKey(up => up.CauseId)
                   .OnDelete(DeleteBehavior.Restrict) // We can't delete an uninteresting post if its cause has been deleted
                   .IsRequired();
        }
    }
}
