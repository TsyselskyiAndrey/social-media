using Glowee.Domain.Entities.NotificationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.HasKey(nt => nt.Id);

            builder.Property(nt => nt.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(nt => nt.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(nt => nt.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(nt => nt.Name)
                   .IsRequired()
                   .HasMaxLength(256);
        }
    }
}
