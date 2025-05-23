using Glowee.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(n => n.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(n => n.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(1024);

            builder.Property(n => n.IsRead)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(n => n.NotificationTypeId)
                   .IsRequired();
            builder.HasOne(n => n.NotificationType)
                   .WithMany(nt => nt.Notifications)
                   .HasForeignKey(n => n.NotificationTypeId)
                   .OnDelete(DeleteBehavior.Restrict) // We can't delete a notification if its type has been deleted
                   .IsRequired();

            builder.Property(n => n.UserId)
                   .IsRequired();
            builder.HasOne(n => n.User)
                   .WithMany(u => u.NotificationsReceived)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(n => n.SenderId)
                   .IsRequired(false);
            builder.HasOne(n => n.Sender)
                   .WithMany(u => u.NotificationsSent)
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            builder.Property(n => n.PostId)
                   .IsRequired(false);
            builder.HasOne(n => n.Post)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            builder.Property(n => n.CommentId)
                   .IsRequired(false);
            builder.HasOne(n => n.Comment)
                   .WithMany(c => c.Notifications)
                   .HasForeignKey(n => n.CommentId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);
        }
    }
}
