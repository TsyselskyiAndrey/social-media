using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Application.MappingProfiles;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Queries;

public class GetSavedPostQueryHandlerTest : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IPostMapper _postMapper;
    private readonly UserRepository _userRepository;

    public GetSavedPostQueryHandlerTest(TestContext fixture)
    {
        _context = fixture.Context;
        _context.SeedData();
        _savedPostRepository = new SavedPostRepository(_context);
        _userRepository = new UserRepository(_context);
        var thumbnailStorageServiceMock = MockThumbnailStorageService.GetThumbnailStorageService();
        var postMediaStorageServiceMock = MockPostMediaStoraheSrvice.GetPostMediaStorageService();
        
        //_postMapper = new PostMapper(thumbnailStorageServiceMock.Object, postMediaStorageServiceMock.Object);
    }

    [Theory]
    [MemberData(nameof(TestData.GetUsersSavedPostsTestData), MemberType = typeof(TestData))]
    public async Task GetSavedPostsTest(long userId, int expectedPosts)
    {
        var command = new GetSavedPostsQuery(new UserId(userId));
        var handler = new GetSavedPostsQueryHandler(_savedPostRepository, _userRepository, _postMapper);

        var data = await handler.Handle(command, new CancellationToken());

        data.Count().ShouldBe(expectedPosts);
    }
    
    [Fact]
    public async Task SavedPostShouldContainCorrectMediaUrls()
    {
        var userId = 1L;
        var query = new GetSavedPostsQuery(new UserId(userId));
        var handler = new GetSavedPostsQueryHandler(_savedPostRepository, _userRepository, _postMapper);

        var result = (await handler.Handle(query, new CancellationToken())).ToList();

        result.ShouldNotBeEmpty();

        var postDto = result.FirstOrDefault(p => p.Id == 1);
        postDto.ShouldNotBeNull();
    
        postDto.PostMediaDtos.ShouldNotBeNull();
        postDto.PostMediaDtos.Count.ShouldBe(2);

        var mediaUrls = postDto.PostMediaDtos.Select(m => m.MediaUrl).ToList();

        mediaUrls.Any(url => url.EndsWith("/media1.jpg")).ShouldBeTrue();
        mediaUrls.Any(url => url.EndsWith("/media2.jpg")).ShouldBeTrue();
    }
}