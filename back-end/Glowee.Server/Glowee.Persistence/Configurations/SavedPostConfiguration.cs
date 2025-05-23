using Glowee.Domain.Entities.SavedPosts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Glowee.Persistence.Configurations
{
    public class SavedPostConfiguration : IEntityTypeConfiguration<SavedPost>
    {
        public void Configure(EntityTypeBuilder<SavedPost> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.Id)
                   .HasConversion(id => id.Value, value => new(value))
                   .ValueGeneratedOnAdd();

            builder.Property(sp => sp.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(sp => sp.ModifiedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

            builder.Property(sp => sp.PostId)
                   .IsRequired();
            builder.HasOne(sp => sp.Post)
                   .WithMany(p => p.SavedPosts)
                   .HasForeignKey(sp => sp.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(sp => sp.UserId)
                   .IsRequired();
            builder.HasOne(sp => sp.User)
                   .WithMany(u => u.SavedPosts)
                   .HasForeignKey(sp => sp.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
