using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Queries.Comments;
using Glowee.Application.Features.Post.Queries.Posts;
using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Application.MappingProfiles;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Queries;

public class GetPostCommentsQueryHandlerTest : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly ICommentMapper _commentMapper;
    private readonly PostRepository _postRepository;
    private readonly CommentsRepository _commentRepository;
    private readonly Mock<IUserService> _userService;

    public GetPostCommentsQueryHandlerTest(TestContext fixture)
    {
        _context = fixture.Context;
        _context.SeedData();
        _postRepository = new PostRepository(_context);
        _commentRepository = new CommentsRepository(_context);
        _userService = MockUserService.GetMockUserService();
        
        var userProfileImage = MockUserProfileImageService.GetMockProfileImageStorageService();
        
        _commentMapper = new CommentMapper(userProfileImage.Object);    
    }
    
    [Theory]
    [MemberData(nameof(TestData.GetPostCommentTestData), MemberType = typeof(TestData))]
    public async Task GetPostCommentTest(long postId, int expectedComments, int expectedParentComments)
    {
        var command = new GetPostCommentsQuery(new PostId(postId));
        var handler = new GetPostCommentsQueryHandler(_commentRepository, _postRepository, _commentMapper, _userService.Object);

        var data = await handler.Handle(command, new CancellationToken());

        var commentDtos = data.ToList();
        commentDtos.Count().ShouldBe(expectedComments);
        commentDtos.Select(x => x.ParentComments?.Count ?? 0).Max().ShouldBe(expectedParentComments);
    }

    [Fact]
    public async Task CommentShouldContainCorrectUserImageUrl()
    {
        long postId = 1L;
        
        var command = new GetPostCommentsQuery(new PostId(postId));
        var handler = new GetPostCommentsQueryHandler(_commentRepository, _postRepository, _commentMapper, _userService.Object);
        
        var data = await handler.Handle(command, new CancellationToken());
        
        var comments = data.ToList();
        var urls = comments.Select(x => x.Author.ProfileImageUrl).ToList();
        urls[0]!.ShouldBe("https://yourdomain.com/images/1.jpg");
    }
}