using Glowee.Domain.Entities.MessageAttachmentTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class MessageAttachmentTypeConfiguration : IEntityTypeConfiguration<MessageAttachmentType>
    {
        public void Configure(EntityTypeBuilder<MessageAttachmentType> builder)
        {
            builder.HasKey(mat => mat.Id);

            builder.Property(mat => mat.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(mat => mat.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(mat => mat.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(mat => mat.Name)
                   .IsRequired()
                   .HasMaxLength(256);
        }
    }
}
