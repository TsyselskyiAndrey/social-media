using Glowee.Domain.Common;
using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(m => m.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(m => m.SenderId)
                   .IsRequired();
            builder.HasOne(m => m.Sender)
                   .WithMany(u => u.MessagesSent)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(4095);

            builder.Property(m => m.RecipientId)
                   .IsRequired(false);
            builder.HasOne(m => m.Recipient)
                   .WithMany(u => u.MessagesReceived)
                   .HasForeignKey(m => m.RecipientId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            builder.Property(m => m.ChatId)
                   .IsRequired();
            builder.HasOne(m => m.Chat)
                   .WithMany(c => c.Messages)
                   .HasForeignKey(m => m.ChatId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
