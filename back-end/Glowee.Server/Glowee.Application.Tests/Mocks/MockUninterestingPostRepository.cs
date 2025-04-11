using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.UninterestingPostCauses;
using Glowee.Domain.Entities.UninterestingPosts;
using Glowee.Domain.Entities.Users;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockUninterestingPostRepository
{
    public static Mock<IUninterestingPostRepository> GetMockUninterestingPostRepository()
    {
        var uninterestingPosts = new List<UninterestingPost>
        {
            new UninterestingPost
            {
                Id = new UninterestingPostId(1),
                PostId = new PostId(1),
                UserId = new UserId(1),
                CauseId = new UninterestingPostCauseId(1),
                Post = new Post
                {
                    Id = new PostId(1),
                    UserId = new UserId(1),
                    Caption = "First post about C#",
                    PostTypeId = new PostTypeId(1),
                },
                User = new User
                {
                    Id = new UserId(1),
                    UserName = "johndoe"
                },
                Cause = new UninterestingPostCause
                {
                    Id = new UninterestingPostCauseId(1),
                    Message = "Cause about C#",
                }
            },
            new UninterestingPost
            {
                Id = new UninterestingPostId(2),
                PostId = new PostId(2),
                UserId = new UserId(2),
                CauseId = new UninterestingPostCauseId(2),
                Post = new Post
                {
                    Id = new PostId(2),
                    UserId = new UserId(2),
                    Caption = "Exploring .NET 8",
                    PostTypeId = new PostTypeId(2),
                },
                User = new User
                {
                    Id = new UserId(2),
                    UserName = "janesmith"
                },
                Cause = new UninterestingPostCause
                {
                    Id = new UninterestingPostCauseId(2),
                    Message = "Cause about C#",
                }
            },
            new UninterestingPost
            {
                Id = new UninterestingPostId(3),
                PostId = new PostId(3),
                UserId = new UserId(1),
                CauseId = new UninterestingPostCauseId(3),
                Post = new Post
                {
                    Id = new PostId(3),
                    UserId = new UserId(3),
                    Caption = "How to use Entity Framework",
                    PostTypeId = new PostTypeId(1),
                },
                User = new User
                {
                    Id = new UserId(1),
                    UserName = "johndoe"
                },
                Cause = new UninterestingPostCause
                {
                    Id = new UninterestingPostCauseId(3),
                    Message = "Cause about C#",
                }
            }
        };

        var mock = new Mock<IUninterestingPostRepository>();

        mock.Setup(repo => repo.GetAsync()).ReturnsAsync(uninterestingPosts);

        mock.Setup(repo => repo.CreateAsync(It.IsAny<UninterestingPost>()))
            .Callback((UninterestingPost post) => uninterestingPosts.Add(post))
            .Returns(Task.CompletedTask);

        return mock;
    }
}