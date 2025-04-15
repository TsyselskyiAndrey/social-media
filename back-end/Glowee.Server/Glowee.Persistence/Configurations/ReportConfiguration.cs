using Glowee.Domain.Entities.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(r => r.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(r => r.IsProcessed)
                   .IsRequired(false);

            builder.Property(r => r.ReportTypeId)
                   .IsRequired();
            builder.HasOne(r => r.ReportType)
                   .WithMany(rt => rt.Reports)
                   .HasForeignKey(r => r.ReportTypeId)
                   .OnDelete(DeleteBehavior.Restrict) // We can't delete a report if its type has been deleted
                   .IsRequired();

            builder.Property(r => r.ReportedUserId)
                   .IsRequired(false);
            builder.HasOne(r => r.ReportedUser)
                   .WithMany(u => u.ReportsReceived)
                   .HasForeignKey(r => r.ReportedUserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            builder.Property(r => r.ComplainantId)
                   .IsRequired();
            builder.HasOne(r => r.Complainant)
                   .WithMany(u => u.ReportsMade)
                   .HasForeignKey(r => r.ComplainantId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(r => r.PostId)
                   .IsRequired(false);
            builder.HasOne(r => r.Post)
                   .WithMany(p => p.Reports)
                   .HasForeignKey(r => r.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            builder.Property(r => r.CommentId)
                   .IsRequired(false);
            builder.HasOne(r => r.Comment)
                   .WithMany(c => c.Reports)
                   .HasForeignKey(r => r.CommentId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);
        }
    }
}
