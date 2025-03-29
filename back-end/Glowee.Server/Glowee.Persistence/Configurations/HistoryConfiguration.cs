using Glowee.Domain.Entities.Histories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class HistoryConfiguration : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(h => h.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(h => h.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(h => h.PostId)
                   .IsRequired();
            builder.HasOne(h => h.Post)
                   .WithMany(p => p.Histories)
                   .HasForeignKey(h => h.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(h => h.UserId)
                   .IsRequired();
            builder.HasOne(h => h.User)
                   .WithMany(u => u.Histories)
                   .HasForeignKey(h => h.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(v => v.Duration)
                   .IsRequired(false);
        }
    }
}
