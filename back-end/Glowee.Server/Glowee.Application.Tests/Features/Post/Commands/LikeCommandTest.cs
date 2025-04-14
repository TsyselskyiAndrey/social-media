using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.Commands.Likes;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Commands;

public class LikeCommandTest : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly IPostRepository _postRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly Mock<IUserRepository> _mockUserRepo;

    public LikeCommandTest(TestContext fixture)
    {
        _context = fixture.Context;       
        _context.SeedData();
        _postRepository = new PostRepository(_context);
        _likeRepository = new LikeRepository(_context);
        _mockUserRepo = MockUserRepository.GetMockUsersRepository();
    }

    [Theory]
    [MemberData(nameof(TestData.GetLikeCommandTestData), MemberType = typeof(TestData))]
    public async Task LikeCommand_Test(long postId, long userId, object expectedResult)
    {
        var handler = new LikeCommandHandler(_likeRepository, _postRepository, _mockUserRepo.Object);
        var command = new LikeCommand(new PostId(postId), new UserId(userId));
        
        if (expectedResult is Type expectedException)
        {
            await Should.ThrowAsync(() => handler.Handle(command, CancellationToken.None), expectedException);
        }
        else
        {
            var result = await handler.Handle(command, CancellationToken.None);
            bool expected = (bool)expectedResult;
            result.ShouldBe(expected);
        }
    }
}