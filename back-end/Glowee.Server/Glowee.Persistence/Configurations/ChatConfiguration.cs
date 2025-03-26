using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(c => c.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(c => c.Name)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.Property(c => c.LogoUrl)
                   .IsRequired(false)
                   .HasMaxLength(512);

            builder.Property(c => c.IsGroup)
                   .IsRequired()
                   .HasDefaultValue(false);
        }
    }
}
