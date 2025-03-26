using Glowee.Domain.Common;
using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
    {
        public void Configure(EntityTypeBuilder<MessageAttachment> builder)
        {
            builder.HasKey(ma => ma.Id);

            builder.Property(ma => ma.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(ma => ma.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(ma => ma.MessageId)
                   .IsRequired();
            builder.HasOne(ma => ma.Message)
                   .WithMany(m => m.MessageAttachments)
                   .HasForeignKey(ma => ma.MessageId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(ma => ma.MessageAttachmentTypeId)
                   .IsRequired();
            builder.HasOne(ma => ma.MessageAttachmentType)
                   .WithMany(mat => mat.MessageAttachments)
                   .HasForeignKey(ma => ma.MessageAttachmentTypeId)
                   .OnDelete(DeleteBehavior.Restrict)  // We can't delete an attachment if its type has been deleted
                   .IsRequired();

            builder.Property(ma => ma.MediaUrl)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(ma => ma.Duration)
                   .IsRequired(false);

            builder.Property(ma => ma.Format)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(ma => ma.Size)
                   .IsRequired();

            builder.Property(ma => ma.IsUploaded)
                   .IsRequired(false);
        }
    }
}
