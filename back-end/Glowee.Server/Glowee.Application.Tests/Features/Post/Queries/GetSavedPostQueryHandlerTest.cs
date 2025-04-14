using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Queries;

public class GetSavedPostQueryHandlerTest : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly Mock<IUserRepository> _userRepository;
    
    public GetSavedPostQueryHandlerTest()
    {
        _context = TestDbConfig<SqlDbContext>.GetContext();
        _context.SeedData();
        _savedPostRepository = new SavedPostRepository(_context);
        _userRepository = MockUserRepository.GetMockUsersRepository();
    }

    [Theory]
    [MemberData(nameof(TestData.GetUsersSavedPostsTestData), MemberType = typeof(TestData))]
    public async Task GetSavedPostsTest(long userId, int expectedPosts)
    {
        var command = new GetSavedPostsQuery(new UserId(userId));
        var handler = new GetSavedPostsQueryHandler(_savedPostRepository, _userRepository.Object);
        
        var data = await handler.Handle(command, new CancellationToken());
        
        data.Count().ShouldBe(expectedPosts);
    }
}