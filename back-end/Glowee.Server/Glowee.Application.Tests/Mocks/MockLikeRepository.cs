using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Common;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public static class MockLikeRepository
{
    public static Mock<ILikeRepository> GetMockRepository()
    {
        var likes = new List<LikedPost>
        {
            new LikedPost { PostId = new PostId(1), UserId = new UserId(1) },
            new LikedPost { PostId = new PostId(2), UserId = new UserId(2) },
            new LikedPost { PostId = new PostId(3), UserId = new UserId(3) },
            new LikedPost { PostId = new PostId(4), UserId = new UserId(4) }
        };

        var mock = new Mock<ILikeRepository>();
        
        mock.Setup(x => x.GetAsync()).ReturnsAsync(() => likes);

        mock.Setup(x => x.CreateAsync(It.IsAny<LikedPost>()))
            .Callback((LikedPost p) => likes.Add(p))
            .Returns(Task.CompletedTask);
        
        return mock;
    }
}