using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class NotificationSettingConfiguration : IEntityTypeConfiguration<NotificationSetting>
    {
        public void Configure(EntityTypeBuilder<NotificationSetting> builder)
        {
            builder.HasKey(ns => ns.Id);

            builder.Property(ns => ns.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(ns => ns.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(ns => ns.UserId)
                   .IsRequired();
            builder.HasIndex(ns => ns.UserId, "IX_NotificationSettings_UserId")
                   .IsUnique();
            builder.HasOne(ns => ns.User)
                   .WithOne(u => u.NotificationSettings)
                   .HasForeignKey<NotificationSetting>(ns => ns.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(ns => ns.NotifyPostLikes)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(ns => ns.NotifyComments)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(ns => ns.NotifyReplies)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(ns => ns.NotifyFollows)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(ns => ns.NotifyMessages)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(ns => ns.NotifyMentions)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
