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
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

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

            builder.Property(s => s.StripePriceId)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(s => s.StripeProductId)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(s => s.Currency)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(s => s.Interval)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.Description)
                   .IsRequired()
                   .HasMaxLength(4095);

            builder.Property(s => s.IsActive)
                   .IsRequired()
                   .HasDefaultValue(false);

        }
    }
}
