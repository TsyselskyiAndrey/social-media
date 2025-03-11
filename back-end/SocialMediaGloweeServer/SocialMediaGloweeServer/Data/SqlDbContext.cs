using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialMediaGloweeServer.Models;
using System;

namespace SocialMediaGloweeServer.Data
{
    public class SqlDbContext : IdentityDbContext<User, IdentityRole<int>, int>
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UninterestingPost> UninterestingPosts { get; set; }
        public DbSet<UninterestingPostCause> UninterestingPostCauses { get; set; }
        public DbSet<UsersChat> UsersChats { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.Handle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(c => c.Handle, "IX_Users_Handle")
                   .IsUnique();

                entity.Property(c => c.Biography)
                    .IsRequired(false)
                    .HasMaxLength(1024);

                entity.Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(u => u.BannedUntil)
                    .IsRequired()
                    .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                entity.Property(c => c.ProfileImageUrl)
                    .IsRequired(false)
                    .HasMaxLength(512);

                entity.Property(c => c.BirthDate)
                    .IsRequired(false);

                entity.Property(c => c.EmailConfirmationCode)
                    .IsRequired(false)
                    .HasMaxLength(15);

                entity.Property(u => u.EmailConfirmationCodeExpiryTime)
                    .IsRequired()
                    .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(c => c.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(c => c.LogoUrl)
                   .IsRequired(false)
                   .HasMaxLength(512);

                entity.Property(c => c.IsGroup)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.UserId)
                      .IsRequired();
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(c => c.PostId)
                      .IsRequired();
                entity.HasOne(c => c.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Restrict)  // ----------- Attention
                      .IsRequired();

                entity.Property(c => c.Content)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(c => c.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(c => c.ParentCommentId)
                      .IsRequired(false);
                entity.HasOne(c => c.ParentComment)
                      .WithMany(c => c.ChildComments)
                      .HasForeignKey(c => c.ParentCommentId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired(false);

            });

            modelBuilder.Entity<CommentStatus>(entity =>
            {
                entity.HasKey(cs => cs.Id);

                entity.Property(cs => cs.UserId)
                      .IsRequired();
                entity.HasOne(cs => cs.User)
                      .WithMany(u => u.CommentStatuses)
                      .HasForeignKey(cs => cs.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(cs => cs.CommentId)
                      .IsRequired();
                entity.HasOne(cs => cs.Comment)
                      .WithMany(c => c.CommentStatuses)
                      .HasForeignKey(cs => cs.CommentId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(cs => cs.IsLiked)
                    .IsRequired()
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.FollowerId)
                      .IsRequired();
                entity.HasOne(f => f.Follower)
                      .WithMany(u => u.Followings)
                      .HasForeignKey(f => f.FollowerId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(f => f.FollowedId)
                      .IsRequired();
                entity.HasOne(f => f.Followed)
                      .WithMany(u => u.Followers)
                      .HasForeignKey(f => f.FollowedId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(f => f.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<GeneralSetting>(entity =>
            {
                entity.HasKey(gs => gs.Id);

                entity.Property(gs => gs.UserId)
                      .IsRequired();
                entity.HasIndex(gs => gs.UserId, "IX_GeneralSettings_UserId")
                    .IsUnique();
                entity.HasOne(gs => gs.User)
                      .WithOne(u => u.GeneralSettings)
                      .HasForeignKey<GeneralSetting>(gs => gs.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(gs => gs.IsPrivate)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.Property(gs => gs.Theme)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Light");

                entity.Property(gs => gs.Language)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Englush");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.PostId)
                      .IsRequired();
                entity.HasOne(h => h.Post)
                      .WithMany(p => p.Histories)
                      .HasForeignKey(h => h.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(h => h.UserId)
                      .IsRequired();
                entity.HasOne(h => h.User)
                      .WithMany(u => u.Histories)
                      .HasForeignKey(h => h.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(h => h.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

                entity.Property(v => v.Duration)
                    .IsRequired(false);
            });

            modelBuilder.Entity<LikedPost>(entity =>
            {
                entity.HasKey(lp => lp.Id);

                entity.Property(lp => lp.PostId)
                      .IsRequired();
                entity.HasOne(lp => lp.Post)
                      .WithMany(p => p.LikedPosts)
                      .HasForeignKey(lp => lp.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(lp => lp.UserId)
                      .IsRequired();
                entity.HasOne(lp => lp.User)
                      .WithMany(u => u.LikedPosts)
                      .HasForeignKey(lp => lp.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(lp => lp.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.SenderId)
                      .IsRequired();
                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.MessagesSent)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(c => c.Content)
                    .IsRequired()
                    .HasMaxLength(4096);

                entity.Property(m => m.RecipientId)
                      .IsRequired(false);
                entity.HasOne(m => m.Recipient)
                      .WithMany(u => u.MessagesReceived)
                      .HasForeignKey(m => m.RecipientId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

                entity.Property(m => m.ChatId)
                      .IsRequired();
                entity.HasOne(m => m.Chat)
                      .WithMany(c => c.Messages)
                      .HasForeignKey(m => m.ChatId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(m => m.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<MessageAttachment>(entity =>
            {
                entity.HasKey(ma => ma.Id);

                entity.Property(ma => ma.MessageId)
                      .IsRequired();
                entity.HasOne(ma => ma.Message)
                      .WithMany(m => m.MessageAttachments)
                      .HasForeignKey(ma => ma.MessageId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(ma => ma.MessageAttachmentTypeId)
                      .IsRequired();
                entity.HasOne(ma => ma.MessageAttachmentType)
                      .WithMany(mat => mat.MessageAttachments)
                      .HasForeignKey(ma => ma.MessageAttachmentTypeId)
                      .OnDelete(DeleteBehavior.Restrict)  // We can't delete an attachment if its type has been deleted
                      .IsRequired();

                entity.Property(ma => ma.MediaUrl)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(ma => ma.Duration)
                    .IsRequired(false);

                entity.Property(ma => ma.Format)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(ma => ma.Size)
                    .IsRequired();

                entity.Property(ma => ma.IsUploaded)
                    .IsRequired(false);

                entity.Property(ma => ma.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<MessageAttachmentType>(entity =>
            {
                entity.HasKey(mat => mat.Id);

                entity.Property(mat => mat.Name)
                       .IsRequired()
                       .HasMaxLength(256);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");

                entity.Property(n => n.Message)
                      .IsRequired()
                      .HasMaxLength(1024);

                entity.Property(n => n.IsRead)
                      .IsRequired()
                      .HasDefaultValue(false);

                entity.Property(n => n.NotificationTypeId)
                      .IsRequired();
                entity.HasOne(n => n.NotificationType)
                      .WithMany(nt => nt.Notifications)
                      .HasForeignKey(n => n.NotificationTypeId)
                      .OnDelete(DeleteBehavior.Restrict) // We can't delete a notification if its type has been deleted
                      .IsRequired();

                entity.Property(n => n.UserId)
                      .IsRequired();
                entity.HasOne(n => n.User)
                      .WithMany(u => u.NotificationsReceived)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(n => n.SenderId)
                      .IsRequired(false);
                entity.HasOne(n => n.Sender)
                      .WithMany(u => u.NotificationsSent)
                      .HasForeignKey(n => n.SenderId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

                entity.Property(n => n.PostId)
                      .IsRequired(false);
                entity.HasOne(n => n.Post)
                      .WithMany(p => p.Notifications)
                      .HasForeignKey(n => n.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

                entity.Property(n => n.CommentId)
                      .IsRequired(false);
                entity.HasOne(n => n.Comment)
                      .WithMany(c => c.Notifications)
                      .HasForeignKey(n => n.CommentId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

            });

            modelBuilder.Entity<NotificationSetting>(entity =>
            {
                entity.HasKey(ns => ns.Id);

                entity.Property(ns => ns.UserId)
                      .IsRequired();
                entity.HasIndex(ns => ns.UserId, "IX_NotificationSettings_UserId")
                    .IsUnique();
                entity.HasOne(ns => ns.User)
                      .WithOne(u => u.NotificationSettings)
                      .HasForeignKey<NotificationSetting>(ns => ns.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(ns => ns.NotifyPostLikes)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.Property(ns => ns.NotifyComments)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.Property(ns => ns.NotifyReplies)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.Property(ns => ns.NotifyFollows)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.Property(ns => ns.NotifyMessages)
                   .IsRequired()
                   .HasDefaultValue(true);

                entity.Property(ns => ns.NotifyMentions)
                   .IsRequired()
                   .HasDefaultValue(true);
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.HasKey(nt => nt.Id);

                entity.Property(nt => nt.Name)
                       .IsRequired()
                       .HasMaxLength(256);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserId)
                      .IsRequired();
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(p => p.Caption)
                      .IsRequired(false)
                      .HasMaxLength(4096);

                entity.Property(p => p.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");

                entity.Property(p => p.PostTypeId)
                      .IsRequired();
                entity.HasOne(p => p.PostType)
                      .WithMany(pt => pt.Posts)
                      .HasForeignKey(p => p.PostTypeId)
                      .OnDelete(DeleteBehavior.Restrict) // We can't delete a post if its type has been deleted
                      .IsRequired();

                entity.HasMany(p => p.Tags)
                      .WithMany(t => t.Posts)
                      .UsingEntity<Dictionary<string, object>>(
                          "TagsPosts",
                          j => j
                              .HasOne<Tag>()
                              .WithMany()
                              .HasForeignKey("TagId")
                              .OnDelete(DeleteBehavior.Cascade),
                          j => j
                              .HasOne<Post>()
                              .WithMany()
                              .HasForeignKey("PostId")
                              .OnDelete(DeleteBehavior.Cascade)
                      ).HasKey("TagId", "PostId");
            });

            modelBuilder.Entity<PostMedia>(entity =>
            {
                entity.HasKey(pm => pm.Id);

                entity.Property(pm => pm.PostId)
                      .IsRequired();
                entity.HasOne(pm => pm.Post)
                      .WithMany(p => p.PostMedias)
                      .HasForeignKey(pm => pm.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(pm => pm.MediaUrl)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(pm => pm.PostMediaTypeId)
                      .IsRequired();
                entity.HasOne(pm => pm.PostMediaType)
                      .WithMany(pmt => pmt.PostMedias)
                      .HasForeignKey(pm => pm.PostMediaTypeId)
                      .OnDelete(DeleteBehavior.Restrict) // We can't delete a PostMedia instance if its type has been deleted
                      .IsRequired();

                entity.Property(pm => pm.ThumbnailUrl)
                    .IsRequired(false)
                    .HasMaxLength(512);

                entity.Property(pm => pm.Duration)
                    .IsRequired(false);

                entity.Property(pm => pm.Format)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(pm => pm.Size)
                    .IsRequired();

                entity.Property(pm => pm.IsUploaded)
                    .IsRequired(false);

                entity.Property(pm => pm.Position)
                    .IsRequired(false);

                entity.Property(pm => pm.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<PostMediaType>(entity =>
            {
                entity.HasKey(pmt => pmt.Id);

                entity.Property(pmt => pmt.Name)
                       .IsRequired()
                       .HasMaxLength(256);
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.HasKey(pt => pt.Id);

                entity.Property(pt => pt.Name)
                       .IsRequired()
                       .HasMaxLength(256);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.UserId)
                      .IsRequired();
                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(rt => rt.DeviceId)
                      .IsRequired()
                      .HasMaxLength(2048);

                entity.Property(rt => rt.Token)
                      .IsRequired()
                      .HasMaxLength(2048);

                entity.Property(rt => rt.TokenExpiryTime)
                      .IsRequired()
                      .HasDefaultValueSql("'0001-01-01T00:00:00.000'");

                entity.Property(rt => rt.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");

                entity.Property(r => r.IsProcessed)
                    .IsRequired(false);

                entity.Property(r => r.ReportTypeId)
                      .IsRequired();
                entity.HasOne(r => r.ReportType)
                      .WithMany(rt => rt.Reports)
                      .HasForeignKey(r => r.ReportTypeId)
                      .OnDelete(DeleteBehavior.Restrict) // We can't delete a report if its type has been deleted
                      .IsRequired();

                entity.Property(r => r.ReportedUserId)
                      .IsRequired(false);
                entity.HasOne(r => r.ReportedUser)
                      .WithMany(u => u.ReportsReceived)
                      .HasForeignKey(r => r.ReportedUserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

                entity.Property(r => r.ComplainantId)
                      .IsRequired();
                entity.HasOne(r => r.Complainant)
                      .WithMany(u => u.ReportsMade)
                      .HasForeignKey(r => r.ComplainantId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(r => r.PostId)
                      .IsRequired(false);
                entity.HasOne(r => r.Post)
                      .WithMany(p => p.Reports)
                      .HasForeignKey(r => r.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

                entity.Property(r => r.CommentId)
                      .IsRequired(false);
                entity.HasOne(r => r.Comment)
                      .WithMany(c => c.Reports)
                      .HasForeignKey(r => r.CommentId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired(false);

            });

            modelBuilder.Entity<ReportType>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Name)
                       .IsRequired()
                       .HasMaxLength(256);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.SenderId)
                      .IsRequired();
                entity.HasOne(r => r.Sender)
                      .WithMany(u => u.RequestsSent)
                      .HasForeignKey(r => r.SenderId)
                      .OnDelete(DeleteBehavior.Restrict) // ----------- Attention
                      .IsRequired();

                entity.Property(r => r.RecipientId)
                      .IsRequired();
                entity.HasOne(r => r.Recipient)
                      .WithMany(u => u.RequestsReceived)
                      .HasForeignKey(r => r.RecipientId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(r => r.RequestTypeId)
                      .IsRequired();
                entity.HasOne(r => r.RequestType)
                      .WithMany(rt => rt.Requests)
                      .HasForeignKey(r => r.RequestTypeId)
                      .OnDelete(DeleteBehavior.Restrict) // We can't delete a request if its type has been deleted
                      .IsRequired();

                entity.Property(r => r.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");

                entity.Property(r => r.IsAccepted)
                      .IsRequired(false);

            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Name)
                       .IsRequired()
                       .HasMaxLength(256);
            });

            modelBuilder.Entity<SavedPost>(entity =>
            {
                entity.HasKey(sp => sp.Id);

                entity.Property(sp => sp.PostId)
                      .IsRequired();
                entity.HasOne(sp => sp.Post)
                      .WithMany(p => p.SavedPosts)
                      .HasForeignKey(sp => sp.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(sp => sp.UserId)
                      .IsRequired();
                entity.HasOne(sp => sp.User)
                      .WithMany(u => u.SavedPosts)
                      .HasForeignKey(sp => sp.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(sp => sp.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(s => s.Price)
                      .IsRequired();

                entity.Property(s => s.Description)
                      .IsRequired()
                      .HasMaxLength(4096);

                entity.Property(s => s.Duration)
                      .IsRequired();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<UninterestingPost>(entity =>
            {
                entity.HasKey(up => up.Id);

                entity.Property(up => up.PostId)
                      .IsRequired();
                entity.HasOne(up => up.Post)
                      .WithMany(p => p.UninterestingPosts)
                      .HasForeignKey(up => up.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(up => up.UserId)
                      .IsRequired();
                entity.HasOne(up => up.User)
                      .WithMany(u => u.UninterestingPosts)
                      .HasForeignKey(up => up.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(up => up.CauseId)
                      .IsRequired();
                entity.HasOne(up => up.Cause)
                      .WithMany(c => c.UninterestingPosts)
                      .HasForeignKey(up => up.CauseId)
                      .OnDelete(DeleteBehavior.Restrict) // We can't delete an uninteresting post if its cause has been deleted
                      .IsRequired();

                entity.Property(up => up.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<UninterestingPostCause>(entity =>
            {
                entity.HasKey(upc => upc.Id);

                entity.Property(upc => upc.Name)
                       .IsRequired()
                       .HasMaxLength(255);
            });

            modelBuilder.Entity<UsersChat>(entity =>
            {
                entity.HasKey(uc => uc.Id);

                entity.Property(uc => uc.UserId)
                      .IsRequired();
                entity.HasOne(uc => uc.User)
                      .WithMany(u => u.UsersChats)
                      .HasForeignKey(uc => uc.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(uc => uc.ChatId)
                      .IsRequired();
                entity.HasOne(uc => uc.Chat)
                      .WithMany(c => c.UsersChats)
                      .HasForeignKey(uc => uc.ChatId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(uc => uc.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");

                entity.Property(uc => uc.IsAdminOrModerator)
                      .IsRequired(false);
            });

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.HasKey(us => us.Id);

                entity.Property(us => us.UserId)
                      .IsRequired();
                entity.HasOne(us => us.User)
                      .WithMany(u => u.UserSubscriptions)
                      .HasForeignKey(us => us.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(us => us.SubscriptionId)
                      .IsRequired();
                entity.HasOne(us => us.Subscription)
                      .WithMany(s => s.UserSubscriptions)
                      .HasForeignKey(us => us.SubscriptionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();

                entity.Property(us => us.ActivationDate)
                   .IsRequired()
                   .HasDefaultValueSql("getdate()");
            });


            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var moderator = new IdentityRole("moderator");
            moderator.NormalizedName = "moderator";

            modelBuilder.Entity<IdentityRole>().HasData(admin, moderator);
        }

    }
}
