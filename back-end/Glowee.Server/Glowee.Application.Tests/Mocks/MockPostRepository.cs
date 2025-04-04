using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Users;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockPostRepository
{
    public static Mock<IPostRepository> GetMockPostRepository()
    {
        var posts = new List<Post>
        {
            new Post
            {
                Id = new PostId(1),
                UserId = new UserId(1),
                Caption = "First post about C#",
                PostTypeId = new PostTypeId(1),
            },
            new Post
            {
                Id = new PostId(2),
                UserId = new UserId(2),
                Caption = "Exploring .NET 8",
                PostTypeId = new PostTypeId(2),
            },
            new Post
            {
                Id = new PostId(3),
                UserId = new UserId(3),
                Caption = "How to use Entity Framework",
                PostTypeId = new PostTypeId(1),
            },
            new Post
            {
                Id = new PostId(4),
                UserId = new UserId(4),
                Caption = "SignalR real-time communication",
                PostTypeId = new PostTypeId(2),
            },
            new Post
            {
                Id = new PostId(5),
                UserId = new UserId(5),
                Caption = "Using MediatR in ASP.NET Core",
                PostTypeId = new PostTypeId(1),
            },
            new Post
            {
                Id = new PostId(6),
                UserId = new UserId(6),
                Caption = "Unit testing with xUnit",
                PostTypeId = new PostTypeId(2),
            }
        };
        
        var mock = new Mock<IPostRepository>();

        mock.Setup(repo => repo.GetAsync()).ReturnsAsync(posts);

        mock.Setup(repo => repo.GetByIdAsync(It.IsAny<PostId>()))
            .ReturnsAsync((PostId id) => posts.FirstOrDefault(p => p.Id == id));

        mock.Setup(repo => repo.CreateAsync(It.IsAny<Post>()))
            .Callback((Post post) => posts.Add(post))
            .Returns(Task.CompletedTask);

        return mock;
    }
}