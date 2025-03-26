using Glowee.Domain.Common;
using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class GeneralSettingConfiguration : IEntityTypeConfiguration<GeneralSetting>
    {
        public void Configure(EntityTypeBuilder<GeneralSetting> builder)
        {
            builder.HasKey(gs => gs.Id);

            builder.Property(gs => gs.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(gs => gs.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(gs => gs.UserId)
                   .IsRequired();
            builder.HasIndex(gs => gs.UserId, "IX_GeneralSettings_UserId")
                   .IsUnique();
            builder.HasOne(gs => gs.User)
                   .WithOne(u => u.GeneralSettings)
                   .HasForeignKey<GeneralSetting>(gs => gs.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(gs => gs.IsPrivate)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(gs => gs.Theme)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Light");

            builder.Property(gs => gs.Language)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Englush");
        }
    }
}
