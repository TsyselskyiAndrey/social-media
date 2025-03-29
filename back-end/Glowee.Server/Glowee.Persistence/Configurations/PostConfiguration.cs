using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(p => p.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(p => p.UserId)
                   .IsRequired();
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(p => p.Caption)
                   .IsRequired(false)
                   .HasMaxLength(4095);

            builder.Property(p => p.PostTypeId)
                   .IsRequired();
            builder.HasOne(p => p.PostType)
                   .WithMany(pt => pt.Posts)
                   .HasForeignKey(p => p.PostTypeId)
                   .OnDelete(DeleteBehavior.Restrict) // We can't delete a post if its type has been deleted
                   .IsRequired();

            builder.HasMany(p => p.Tags)
                   .WithMany(t => t.Posts)
                   .UsingEntity<Dictionary<string, object>>(
                       "TagsPosts",
                       j => j
                           .HasOne<Tag>()
                           .WithMany()
                           .HasForeignKey("TagId")
                           .OnDelete(DeleteBehavior.Cascade),
                       j => j
                           .HasOne<Post>()
                           .WithMany()
                           .HasForeignKey("PostId")
                           .OnDelete(DeleteBehavior.Cascade)
                   ).HasKey("TagId", "PostId");
        }
    }
}
