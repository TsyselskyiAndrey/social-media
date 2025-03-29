using Glowee.Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(s => s.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(s => s.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(s => s.Price)
                   .IsRequired();

            builder.Property(s => s.Description)
                   .IsRequired()
                   .HasMaxLength(4095);

            builder.Property(s => s.Duration)
                   .IsRequired();
        }
    }
}
