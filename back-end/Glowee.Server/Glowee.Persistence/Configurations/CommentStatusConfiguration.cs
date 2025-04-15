using Glowee.Domain.Entities.CommentStatuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class CommentStatusConfiguration : IEntityTypeConfiguration<CommentStatus>
    {
        public void Configure(EntityTypeBuilder<CommentStatus> builder)
        {
            builder.HasKey(cs => cs.Id);

            builder.Property(cs => cs.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(cs => cs.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(cs => cs.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(cs => cs.UserId)
                   .IsRequired();
            builder.HasOne(cs => cs.User)
                   .WithMany(u => u.CommentStatuses)
                   .HasForeignKey(cs => cs.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(cs => cs.CommentId)
                   .IsRequired();
            builder.HasOne(cs => cs.Comment)
                   .WithMany(c => c.CommentStatuses)
                   .HasForeignKey(cs => cs.CommentId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(cs => cs.IsLiked)
                   .IsRequired()
                   .HasDefaultValue(false);
        }
    }
}
