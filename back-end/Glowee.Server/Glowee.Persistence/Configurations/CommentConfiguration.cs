using Glowee.Domain.Entities.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(c => c.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(c => c.UserId)
                   .IsRequired();
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(c => c.PostId)
                   .IsRequired();
            builder.HasOne(c => c.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(c => c.PostId)
                   .OnDelete(DeleteBehavior.Restrict)  // ----------- Attention
                   .IsRequired();

            builder.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(2047);

            builder.Property(c => c.ParentCommentId)
                   .IsRequired(false);
            builder.HasOne(c => c.ParentComment)
                   .WithMany(c => c.ChildComments)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired(false);
        }
    }
}
