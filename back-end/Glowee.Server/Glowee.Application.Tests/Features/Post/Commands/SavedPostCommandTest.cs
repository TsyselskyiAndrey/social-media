using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Commands.Likes;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Commands;

public class SavedPostCommandTest : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public SavedPostCommandTest(TestContext fixture)
    {
        _context = fixture.Context;
        _context.SeedData();
        _userRepository = new UserRepository(_context);
        _savedPostRepository = new SavedPostRepository(_context);
        _postRepository = new PostRepository(_context);
    }

    [Theory]
    [MemberData(nameof(TestData.GetSavedPostsTestData), MemberType = typeof(TestData))]
    public async Task SavedPostCommand_Test(long postId, long userId, object expectedResult)
    {
        var handler = new SavedPostHandlerCommand(_postRepository, _userRepository, _savedPostRepository);
        var command = new SavedPostCommand(new UserId(userId), new PostId(postId));

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