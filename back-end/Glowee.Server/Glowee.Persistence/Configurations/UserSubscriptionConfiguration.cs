using Glowee.Domain.Entities.UserSubscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.HasKey(us => us.Id);

            builder.Property(us => us.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(us => us.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(us => us.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(us => us.UserId)
                   .IsRequired();
            builder.HasOne(us => us.User)
                   .WithMany(u => u.UserSubscriptions)
                   .HasForeignKey(us => us.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(us => us.SubscriptionId)
                   .IsRequired();
            builder.HasOne(us => us.Subscription)
                   .WithMany(s => s.UserSubscriptions)
                   .HasForeignKey(us => us.SubscriptionId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(s => s.StripeSubscriptionId)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(s => s.Status)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(us => us.StartDate)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(us => us.CurrentPeriodStart)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(us => us.CurrentPeriodEnd)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(us => us.CancelAt)
                   .IsRequired(false);

            builder.Property(us => us.CanceledAt)
                   .IsRequired(false);

            builder.Property(us => us.IsActive)
                   .IsRequired()
                   .HasDefaultValue(false);
        }
    }
}
