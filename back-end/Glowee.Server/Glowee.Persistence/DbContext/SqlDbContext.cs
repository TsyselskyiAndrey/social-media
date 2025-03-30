using Glowee.Domain.Common;
using Glowee.Domain.Entities.Chats;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.CommentStatuses;
using Glowee.Domain.Entities.Follows;
using Glowee.Domain.Entities.GeneralSettings;
using Glowee.Domain.Entities.Histories;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.MessageAttachments;
using Glowee.Domain.Entities.MessageAttachmentTypes;
using Glowee.Domain.Entities.Messages;
using Glowee.Domain.Entities.Notifications;
using Glowee.Domain.Entities.NotificationSettings;
using Glowee.Domain.Entities.NotificationTypes;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.PostMediaTypes;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Reports;
using Glowee.Domain.Entities.ReportTypes;
using Glowee.Domain.Entities.Requests;
using Glowee.Domain.Entities.RequestTypes;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Subscriptions;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.UninterestingPostCauses;
using Glowee.Domain.Entities.UninterestingPosts;
using Glowee.Domain.Entities.UserChats;
using Glowee.Domain.Entities.UserSubscriptions;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaGloweeServer.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {

        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentStatus> CommentStatuses { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<LikedPost> LikedPosts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageAttachment> MessageAttachments { get; set; }
        public DbSet<MessageAttachmentType> MessageTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSetting> NotificationSettings { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<PostMediaType> PostMediaTypes { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UninterestingPost> UninterestingPosts { get; set; }
        public DbSet<UninterestingPostCause> UninterestingPostCauses { get; set; }
        public DbSet<UserChat> UsersChats { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);
            modelBuilder.HasDefaultSchema("business_schema");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<IEntity>().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.ModifiedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
