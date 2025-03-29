using Glowee.Domain.Entities.PostMedias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class PostMediaConfiguration : IEntityTypeConfiguration<PostMedia>
    {
        public void Configure(EntityTypeBuilder<PostMedia> builder)
        {
            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(pm => pm.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(pm => pm.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(pm => pm.PostId)
                   .IsRequired();
            builder.HasOne(pm => pm.Post)
                   .WithMany(p => p.PostMedias)
                   .HasForeignKey(pm => pm.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(pm => pm.MediaUrl)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(pm => pm.PostMediaTypeId)
                   .IsRequired();
            builder.HasOne(pm => pm.PostMediaType)
                   .WithMany(pmt => pmt.PostMedias)
                   .HasForeignKey(pm => pm.PostMediaTypeId)
                   .OnDelete(DeleteBehavior.Restrict) // We can't delete a PostMedia instance if its type has been deleted
                   .IsRequired();

            builder.Property(pm => pm.ThumbnailUrl)
                   .IsRequired(false)
                   .HasMaxLength(512);

            builder.Property(pm => pm.Duration)
                   .IsRequired(false);

            builder.Property(pm => pm.Format)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(pm => pm.Size)
                   .IsRequired();

            builder.Property(pm => pm.IsUploaded)
                   .IsRequired(false);

            builder.Property(pm => pm.Position)
                   .IsRequired(false);
        }
    }
}
