using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.PostMediaTypes;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.UninterestingPostCauses;
using Glowee.Domain.Entities.UninterestingPosts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;

namespace Glowee.Application.Tests.Data;

public static class GloweeDbSeedData
{
    public static async Task SeedData(this SqlDbContext context)
    {
        // Надо потом добавить пользователей, и внести их в контекст

        if (!context.Posts.Any())
        {
            var posts = new List<Post>
            {
                new Post
                {
                    Id = new PostId(1),
                    UserId = new UserId(1),
                    Caption = "Привет, мир!",
                    PostTypeId = new PostTypeId(1)
                },
                new Post
                {
                    Id = new PostId(2),
                    UserId = new UserId(1),
                    Caption = "Наслаждаюсь природой 🌿",
                    PostTypeId = new PostTypeId(2)
                },
                new Post
                {
                    Id = new PostId(3),
                    UserId = new UserId(2),
                    Caption = "Новое фото профиля",
                    PostTypeId = new PostTypeId(1)
                },
                new Post
                {
                    Id = new PostId(4),
                    UserId = new UserId(3),
                    Caption = "Люблю путешествовать!",
                    PostTypeId = new PostTypeId(1)
                },
                new Post
                {
                    Id = new PostId(5),
                    UserId = new UserId(4),
                    Caption = "Всем хорошего дня ☀️",
                    PostTypeId = new PostTypeId(2)
                },
                new Post
                {
                    Id = new PostId(6),
                    UserId = new UserId(2),
                    Caption = "Новая заметка в блоге",
                    PostTypeId = new PostTypeId(4)
                },
                new Post
                {
                    Id = new PostId(7),
                    UserId = new UserId(3),
                    Caption = "Пробую новую камеру 📷",
                    PostTypeId = new PostTypeId(1)
                },
                new Post
                {
                    Id = new PostId(8),
                    UserId = new UserId(5),
                    Caption = "Какая вкуснятина 😋",
                    PostTypeId = new PostTypeId(1)
                },
                new Post
                {
                    Id = new PostId(9),
                    UserId = new UserId(5),
                    Caption = "День на пляже 🏖️",
                    PostTypeId = new PostTypeId(1)
                },
                new Post
                {
                    Id = new PostId(10),
                    UserId = new UserId(3),
                    Caption = "Книги — лучший друг 📚",
                    PostTypeId = new PostTypeId(5)
                }
            };
            context.Posts.AddRange(posts);
        }

        if (!context.PostTypes.Any())
        {
            var postTypes = new List<PostType>
            {
                new PostType
                {
                    Id = new PostTypeId(1),
                    Name = "Фото"
                },
                new PostType
                {
                    Id = new PostTypeId(2),
                    Name = "Видео"
                },
                new PostType
                {
                    Id = new PostTypeId(3),
                    Name = "История"
                },
                new PostType
                {
                    Id = new PostTypeId(4),
                    Name = "Репост"
                },
                new PostType
                {
                    Id = new PostTypeId(5),
                    Name = "Цитата"
                }
            };

            context.PostTypes.AddRange(postTypes);
        }

        if (!context.Tags.Any())
        {
            var tags = new List<Tag>
            {
                new Tag
                {
                    Id = new TagId(1),
                    Name = "Природа"
                },
                new Tag
                {
                    Id = new TagId(2),
                    Name = "Путешествия"
                },
                new Tag
                {
                    Id = new TagId(3),
                    Name = "Еда"
                },
                new Tag
                {
                    Id = new TagId(4),
                    Name = "Книги"
                },
                new Tag
                {
                    Id = new TagId(5),
                    Name = "Фотография"
                }
            };

            context.Tags.AddRange(tags);
        }

        if (!context.SavedPosts.Any())
        {
            var savedPosts = new List<SavedPost>
            {
                new SavedPost
                {
                    Id = new SavedPostId(1),
                    PostId = new PostId(1),
                    UserId = new UserId(1)
                },
                new SavedPost
                {
                    Id = new SavedPostId(2),
                    PostId = new PostId(2),
                    UserId = new UserId(1)
                },
                new SavedPost
                {
                    Id = new SavedPostId(3),
                    PostId = new PostId(3),
                    UserId = new UserId(1)
                },
                new SavedPost
                {
                    Id = new SavedPostId(4),
                    PostId = new PostId(4),
                    UserId = new UserId(4)
                },
                new SavedPost
                {
                    Id = new SavedPostId(5),
                    PostId = new PostId(5),
                    UserId = new UserId(5)
                }
            };

            context.SavedPosts.AddRange(savedPosts);
        }

        if (!context.PostMedias.Any())
        {
            var postMediaTypes = new List<PostMediaType>
            {
                new PostMediaType { Id = new PostMediaTypeId(1), Name = "Фото" },
                new PostMediaType { Id = new PostMediaTypeId(2), Name = "Видео" },
                new PostMediaType { Id = new PostMediaTypeId(3), Name = "Аудио" }
            };

            context.PostMediaTypes.AddRange(postMediaTypes);

            var postMedias = new List<PostMedia>
            {
                new PostMedia
                {
                    Id = new PostMediaId(1),
                    PostId = new PostId(1),
                    MediaPath = "https://example.com/media1.jpg",
                    PostMediaTypeId = new PostMediaTypeId(1),
                    Format = "jpg",
                    Size = 1024,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(2),
                    PostId = new PostId(1),
                    MediaPath = "https://example.com/media2.jpg",
                    PostMediaTypeId = new PostMediaTypeId(1),
                    Format = "jpg",
                    Size = 2048,
                    IsUploaded = true,
                    Position = 2
                },
                new PostMedia
                {
                    Id = new PostMediaId(3),
                    PostId = new PostId(2),
                    MediaPath = "https://example.com/video1.mp4",
                    PostMediaTypeId = new PostMediaTypeId(2),
                    Format = "mp4",
                    Size = 4096,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(4),
                    PostId = new PostId(3),
                    MediaPath = "https://example.com/media3.jpg",
                    PostMediaTypeId = new PostMediaTypeId(1),
                    Format = "jpg",
                    Size = 512,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(5),
                    PostId = new PostId(4),
                    MediaPath = "https://example.com/video2.mp4",
                    PostMediaTypeId = new PostMediaTypeId(2),
                    Format = "mp4",
                    Size = 8192,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(6),
                    PostId = new PostId(5),
                    MediaPath = "https://example.com/media4.jpg",
                    PostMediaTypeId = new PostMediaTypeId(1),
                    Format = "jpg",
                    Size = 1024,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(7),
                    PostId = new PostId(6),
                    MediaPath = "https://example.com/media5.jpg",
                    PostMediaTypeId = new PostMediaTypeId(1),
                    Format = "jpg",
                    Size = 2048,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(8),
                    PostId = new PostId(7),
                    MediaPath = "https://example.com/video3.mp4",
                    PostMediaTypeId = new PostMediaTypeId(2),
                    Format = "mp4",
                    Size = 10240,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(9),
                    PostId = new PostId(8),
                    MediaPath = "https://example.com/media6.jpg",
                    PostMediaTypeId = new PostMediaTypeId(1),
                    Format = "jpg",
                    Size = 512,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(10),
                    PostId = new PostId(9),
                    MediaPath = "https://example.com/video4.mp4",
                    PostMediaTypeId = new PostMediaTypeId(2),
                    Format = "mp4",
                    Size = 16384,
                    IsUploaded = true,
                    Position = 1
                },
                new PostMedia
                {
                    Id = new PostMediaId(11),
                    PostId = new PostId(10),
                    MediaPath = "https://example.com/audio1.mp3",
                    PostMediaTypeId = new PostMediaTypeId(3),
                    Format = "mp3",
                    Size = 5120,
                    IsUploaded = true,
                    Position = 1
                }
            };

            context.PostMedias.AddRange(postMedias);
        }

        if (!context.LikedPosts.Any())
        {
            var likedPosts = new List<LikedPost>
            {
                new LikedPost { Id = new LikedPostId(1), PostId = new PostId(1), UserId = new UserId(1) },
                new LikedPost { Id = new LikedPostId(2), PostId = new PostId(2), UserId = new UserId(2) },
                new LikedPost { Id = new LikedPostId(3), PostId = new PostId(3), UserId = new UserId(3) },
                new LikedPost { Id = new LikedPostId(4), PostId = new PostId(4), UserId = new UserId(4) },
                new LikedPost { Id = new LikedPostId(5), PostId = new PostId(5), UserId = new UserId(5) },
                new LikedPost { Id = new LikedPostId(6), PostId = new PostId(6), UserId = new UserId(1) },
                new LikedPost { Id = new LikedPostId(7), PostId = new PostId(7), UserId = new UserId(2) },
                new LikedPost { Id = new LikedPostId(8), PostId = new PostId(8), UserId = new UserId(3) },
                new LikedPost { Id = new LikedPostId(9), PostId = new PostId(9), UserId = new UserId(4) },
                new LikedPost { Id = new LikedPostId(10), PostId = new PostId(10), UserId = new UserId(5) }
            };

            context.LikedPosts.AddRange(likedPosts);
        }

        if (!context.UninterestingPostCauses.Any())
        {
            var uninterestingPostCauses = new List<UninterestingPostCause>
            {
                new UninterestingPostCause { Id = new UninterestingPostCauseId(1), Name = "Низкое качество", Message = "Пост имеет плохое качество изображения или видео" },
                new UninterestingPostCause { Id = new UninterestingPostCauseId(2), Name = "Повторяется", Message = "Пост слишком похож на другие" },
                new UninterestingPostCause { Id = new UninterestingPostCauseId(3), Name = "Неинтересная тема", Message = "Тема поста не вызывает интерес" }
            };

            context.UninterestingPostCauses.AddRange(uninterestingPostCauses);
        }

        if (!context.UninterestingPosts.Any())
        {
            var uninterestingPosts = new List<UninterestingPost>
            {
                new UninterestingPost { Id = new UninterestingPostId(1), PostId = new PostId(1), UserId = new UserId(2), CauseId = new UninterestingPostCauseId(1) },
                new UninterestingPost { Id = new UninterestingPostId(2), PostId = new PostId(2), UserId = new UserId(3), CauseId = new UninterestingPostCauseId(2) },
                new UninterestingPost { Id = new UninterestingPostId(3), PostId = new PostId(3), UserId = new UserId(4), CauseId = new UninterestingPostCauseId(3) },
                new UninterestingPost { Id = new UninterestingPostId(4),PostId = new PostId(4), UserId = new UserId(5), CauseId = new UninterestingPostCauseId(1) },
                new UninterestingPost { Id = new UninterestingPostId(5), PostId = new PostId(5), UserId = new UserId(1), CauseId = new UninterestingPostCauseId(2) }
            };

            context.UninterestingPosts.AddRange(uninterestingPosts);
        }

        if (!context.Comments.Any())
        {
            var comments = new List<Comment>()
            {
                new Comment()
                {
                    Id = new CommentId(1),
                    PostId = new PostId(1),
                    UserId = new UserId(1),
                    Content = "IMPRESSIVE im really happy",
                },
                new Comment()
                {
                    Id = new CommentId(2),
                    PostId = new PostId(1),
                    UserId = new UserId(2),
                    Content = "Bruh",
                },
                new Comment()
                {
                    Id = new CommentId(3),
                    PostId = new PostId(1),
                    UserId = new UserId(1),
                    Content = "Really bruh",
                },
                new Comment()
                {
                    Id = new CommentId(4),
                    PostId = new PostId(2),
                    UserId = new UserId(3),
                    Content = "I am good",
                },
                new Comment()
                {
                    Id = new CommentId(5),
                    PostId = new PostId(2),
                    UserId = new UserId(5),
                    Content = "I am bad",
                }
            };
        }

        // Зачем поле IsLiked если потом постоянно надо будет создавать , можно проверять по наличию
        if (!context.CommentStatuses.Any())
        {

        }

        await context.SaveChangesAsync();
    }
}