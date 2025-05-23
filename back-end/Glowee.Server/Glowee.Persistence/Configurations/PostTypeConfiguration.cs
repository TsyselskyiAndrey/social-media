using Glowee.Domain.Entities.PostTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class PostTypeConfiguration : IEntityTypeConfiguration<PostType>
    {
        public void Configure(EntityTypeBuilder<PostType> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(pt => pt.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(pt => pt.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(pt => pt.Name)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasData(
                new PostType()
                {
                    Id = new PostTypeId(1),
                    Name = "Photo",
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
                new PostType()
                {
                    Id = new PostTypeId(2),
                    Name = "Video",
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                },
                new PostType()
                {
                    Id = new PostTypeId(3),
                    Name = "Carousel",
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                }
                );
        }
    }
}
