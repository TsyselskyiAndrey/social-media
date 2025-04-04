using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.Commands.Likes;
using Glowee.Application.Tests.Mocks;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Commands;

public class LikeCommandTest
{
    private readonly Mock<ILikeRepository> _mockLikeRepo;
    private readonly Mock<IPostRepository> _mockPostRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;

    public LikeCommandTest()
    {
        _mockLikeRepo = MockLikeRepository.GetMockRepository();
        _mockPostRepo = MockPostRepository.GetMockPostRepository();
        _mockUserRepo = MockUserRepository.GetMockUsersRepository();
    }

    [Theory]
    [MemberData(nameof(GetLikeCommandTestData))]
    public async Task CreateLikeCommand_CreatesLikeTest(long postId, long userId, object expectedResult)
    {
        var handler = new LikeCommandHandler(_mockLikeRepo.Object, _mockPostRepo.Object, _mockUserRepo.Object);
        var command = new LikeCommand(new PostId(postId), new UserId(userId));

        if (expectedResult is Type expectedException)
        {
            await Should.ThrowAsync(() => handler.Handle(command, CancellationToken.None), expectedException);
        }
        else
        {
            var result = await handler.Handle(command, CancellationToken.None);
            bool expected = (bool)expectedResult;
            result.Result.ShouldBe(expected);
        }
    }


    public static IEnumerable<object[]> GetLikeCommandTestData()
    {
        yield return [1, 1, false];
        yield return [2, 2, false];
        yield return [3, 3, false];
        yield return [4, 4, false];
        yield return [5, 5, true]; 
        yield return [6, 6, true];
        yield return [1, 999, typeof(NotFoundException)];
        yield return [999, 999, typeof(NotFoundException)];
    }
}