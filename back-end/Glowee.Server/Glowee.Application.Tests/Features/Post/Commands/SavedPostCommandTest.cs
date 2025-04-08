using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.Commands.Likes;
using Glowee.Application.Tests.Mocks;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Commands;

public class SavedPostCommandTest
{
    private readonly Mock<ISavedPostRepository> _mockSavedPostRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IPostRepository> _mockPostRepo;

    public SavedPostCommandTest()
    {
        _mockUserRepo = MockUserRepository.GetMockUsersRepository();
        _mockPostRepo = MockPostRepository.GetMockPostRepository();
        _mockSavedPostRepo = MockSavedPostRepository.GetMockSavedPostRepository();
    }

    [Theory]
    [MemberData(nameof(GetSavedPostsTestData))]
    public async Task SavedPostCommand_Test(long userId, long postId, object expectedResult)
    {
        var handler = new SavedPostHandlerCommand(_mockPostRepo.Object, _mockUserRepo.Object, _mockSavedPostRepo.Object);
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

    public static IEnumerable<object[]> GetSavedPostsTestData()
    {
        yield return [1L, 1L, false];
        yield return [2L, 2L, false];
        yield return [3L, 3L, false];
        yield return [4L, 1L, true];
        yield return [5L, 2L, true];
        yield return [6L, 6L, true];
        yield return [999L, 1L, typeof(NotFoundException)];
        yield return [1L, 999L, typeof(NotFoundException)];
        yield return [999L, 999L, typeof(NotFoundException)];
    }
}