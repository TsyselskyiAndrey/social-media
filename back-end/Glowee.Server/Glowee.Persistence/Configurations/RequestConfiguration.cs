using Glowee.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
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

            builder.Property(r => r.SenderId)
                   .IsRequired();
            builder.HasOne(r => r.Sender)
                   .WithMany(u => u.RequestsSent)
                   .HasForeignKey(r => r.SenderId)
                   .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                   .IsRequired();

            builder.Property(r => r.RecipientId)
                   .IsRequired();
            builder.HasOne(r => r.Recipient)
                   .WithMany(u => u.RequestsReceived)
                   .HasForeignKey(r => r.RecipientId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(r => r.RequestTypeId)
                   .IsRequired();
            builder.HasOne(r => r.RequestType)
                   .WithMany(rt => rt.Requests)
                   .HasForeignKey(r => r.RequestTypeId)
                   .OnDelete(DeleteBehavior.Restrict) // We can't delete a request if its type has been deleted
                   .IsRequired();

            builder.Property(r => r.IsAccepted)
                   .IsRequired(false);
        }
    }
}
