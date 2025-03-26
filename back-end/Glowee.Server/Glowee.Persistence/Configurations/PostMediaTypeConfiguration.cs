using Glowee.Domain.Common;
using Glowee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class PostMediaTypeConfiguration : IEntityTypeConfiguration<PostMediaType>
    {
        public void Configure(EntityTypeBuilder<PostMediaType> builder)
        {
            builder.HasKey(pmt => pmt.Id);

            builder.Property(pmt => pmt.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(pmt => pmt.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(pmt => pmt.Name)
                   .IsRequired()
                   .HasMaxLength(256);
        }
    }
}
