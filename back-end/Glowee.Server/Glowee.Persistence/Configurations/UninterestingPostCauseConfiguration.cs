using Glowee.Domain.Entities.UninterestingPostCauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class UninterestingPostCauseConfiguration : IEntityTypeConfiguration<UninterestingPostCause>
    {
        public void Configure(EntityTypeBuilder<UninterestingPostCause> builder)
        {
            builder.HasKey(upc => upc.Id);

            builder.Property(upc => upc.Id)
                   .HasConversion(id => id.Value, value => new(value));

            builder.Property(upc => upc.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(upc => upc.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(upc => upc.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(upc => upc.Message)
                   .IsRequired()
                   .HasMaxLength(4095);
        }
    }
}
