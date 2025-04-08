using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Users;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockSavedPostRepository
{

    public static Mock<ISavedPostRepository> GetMockSavedPostRepository()
    {
        var savedPosts = new List<SavedPost>
        {
            new SavedPost
            {
                Id = new SavedPostId(1),
                PostId = new PostId(1),
                UserId = new UserId(1),
                Post = new Post { Id = new PostId(1), Caption = "C# Post", UserId = new UserId(1) },
                User = new User { Id = new UserId(1), UserName = "johndoe" }
            },
            new SavedPost
            {
                Id = new SavedPostId(2),
                PostId = new PostId(2),
                UserId = new UserId(2),
                Post = new Post { Id = new PostId(2), Caption = ".NET 8 News", UserId = new UserId(2) },
                User = new User { Id = new UserId(2), UserName = "janesmith" }
            },
            new SavedPost
            {
                Id = new SavedPostId(3),
                PostId = new PostId(3),
                UserId = new UserId(3),
                Post = new Post { Id = new PostId(3), Caption = "EF Core Guide", UserId = new UserId(3) },
                User = new User { Id = new UserId(3), UserName = "alicej" }
            }
        };

        var mock = new Mock<ISavedPostRepository>();

        mock.Setup(repo => repo.GetAsync()).ReturnsAsync(savedPosts);

        mock.Setup(repo => repo.CreateAsync(It.IsAny<SavedPost>()))
            .Callback((SavedPost savedPost) => savedPosts.Add(savedPost))
            .Returns(Task.CompletedTask);

        mock.Setup(repo => repo.DeleteAsync(It.IsAny<SavedPost>()))
            .Callback((SavedPost savedPost) => savedPosts.Remove(savedPost))
            .Returns(Task.CompletedTask);

        return mock;
    }
}