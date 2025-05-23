using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Commands.SavedPosts;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Posts;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Commands;

public class SavedPostCommandTest : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IPostRepository _postRepository;
    private readonly Mock<IUserService> _userService;

    public SavedPostCommandTest(TestContext fixture)
    {
        _context = fixture.Context;
        _context.SeedData();
        _savedPostRepository = new SavedPostRepository(_context);
        _userService = MockUserService.GetMockUserService();
    }

    [Theory]
    [MemberData(nameof(TestData.GetSavedPostsTestData), MemberType = typeof(TestData))]
    public async Task SavedPostCommand_Test(long postId, long userId, object expectedResult)
    {
        var handler = new SavedPostCommandHandler(_postRepository,  _savedPostRepository, _userService.Object);
        var command = new SavedPostCommand(new PostId(postId));

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