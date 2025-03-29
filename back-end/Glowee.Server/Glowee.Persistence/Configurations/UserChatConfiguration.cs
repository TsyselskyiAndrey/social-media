using Glowee.Domain.Entities.UserChats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class UserChatConfiguration : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.HasKey(uc => uc.Id);

            builder.Property(uc => uc.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(uc => uc.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(uc => uc.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(uc => uc.UserId)
                   .IsRequired();
            builder.HasOne(uc => uc.User)
                   .WithMany(u => u.UsersChats)
                   .HasForeignKey(uc => uc.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(uc => uc.ChatId)
                   .IsRequired();
            builder.HasOne(uc => uc.Chat)
                   .WithMany(c => c.UsersChats)
                   .HasForeignKey(uc => uc.ChatId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(uc => uc.IsAdminOrModerator)
                   .IsRequired(false);
        }
    }
}
