using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.PostMediaTypes;
using Glowee.Domain.Entities.Posts;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockPostMediaRepository
{
    public static Mock<IPostMediaRepository> GetMockPostMediaRepository()
    {
        var postMedias = new List<PostMedia>
        {
            new PostMedia
            {
                Id = new PostMediaId(1),
                PostId = new PostId(1),
                MediaUrl = "https://cdn.example.com/media/post1-image.jpg",
                ThumbnailUrl = "https://cdn.example.com/media/thumbnails/post1-thumb.jpg",
                PostMediaTypeId = new PostMediaTypeId(1),
                PostMediaType = new PostMediaType
                {
                    Id = new PostMediaTypeId(1),
                    Name = "Image"
                },
                Format = "jpg",
                Size = 204800,
                IsUploaded = true,
                Position = 1
            },
            new PostMedia
            {
                Id = new PostMediaId(2),
                PostId = new PostId(2),
                MediaUrl = "https://cdn.example.com/media/post2-video.mp4",
                ThumbnailUrl = "https://cdn.example.com/media/thumbnails/post2-thumb.jpg",
                PostMediaTypeId = new PostMediaTypeId(2),
                PostMediaType = new PostMediaType
                {
                    Id = new PostMediaTypeId(2),
                    Name = "Video"
                },
                Duration = TimeSpan.FromSeconds(90),
                Format = "mp4",
                Size = 5242880,
                IsUploaded = true,
                Position = 1
            },
            new PostMedia
            {
                Id = new PostMediaId(3),
                PostId = new PostId(3),
                MediaUrl = "https://cdn.example.com/media/post3-gif.gif",
                ThumbnailUrl = "https://cdn.example.com/media/thumbnails/post3-thumb.jpg",
                PostMediaTypeId = new PostMediaTypeId(3),
                PostMediaType = new PostMediaType
                {
                    Id = new PostMediaTypeId(3),
                    Name = "GIF"
                },
                Format = "gif",
                Size = 1024000,
                IsUploaded = true,
                Position = 1
            }
        };

        var mock = new Mock<IPostMediaRepository>();

        mock.Setup(repo => repo.GetAsync()).ReturnsAsync(postMedias);

        mock.Setup(repo => repo.GetByIdAsync(It.IsAny<PostMediaId>()))
            .ReturnsAsync((PostMediaId id) => postMedias.FirstOrDefault(m => m.Id == id));

        mock.Setup(repo => repo.CreateAsync(It.IsAny<PostMedia>()))
            .Callback((PostMedia media) => postMedias.Add(media))
            .Returns(Task.CompletedTask);

        return mock;
    }
}