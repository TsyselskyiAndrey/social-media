using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Commands.Comment.CreateComment;
using Glowee.Application.Features.Post.Commands.Comment.DeleteComment;
using Glowee.Application.Features.Post.Commands.Comment.EditComment;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Comments;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Moq;
using Shouldly;

namespace Glowee.Application.Tests.Features.Post.Commands;

public class CommentCrudCommandsTests : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly Mock<IUserService> _userService;
    private readonly ICommentsRepository _commentsRepository;
    private readonly PostRepository _postRepository;

    public CommentCrudCommandsTests(TestContext context)
    {
        _context = context.Context;
        _context.SeedData();
        _userService = MockUserService.GetMockUserService();
        _commentsRepository = new CommentsRepository(_context);
        //_postRepository = new PostRepository(_context);
    }

    [Theory]
    [MemberData(nameof(TestData.CrateValidCommentsTestData), MemberType = typeof(TestData))]
    public async Task TestAddCommentToPost(object createCommentCommand)
    {
        var comment = createCommentCommand as CreateCommentCommand;
        var handler = new CreateCommentCommandHandler(_userService.Object, _postRepository, _commentsRepository);

        await handler.Handle(comment!, new CancellationToken());

        var comments = await _commentsRepository.GetAsync();
        
        comments.FirstOrDefault(x => x.Content == comment?.Content && x.PostId.Value == comment.PostId).ShouldNotBeNull();
    }
        
    [Theory]
    [MemberData(nameof(TestData.CreateInvalidCommentsTestData), MemberType = typeof(TestData))]
    public async Task CreateComment_ShouldThrowException_WhenInvalid(CreateCommentCommand command, Type expectedException)
    {
        var handler = new CreateCommentCommandHandler(_userService.Object, _postRepository, _commentsRepository);

        var act = () => handler.Handle(command, new CancellationToken());

        await Should.ThrowAsync(act, expectedException);
    }

    [Theory]
    [MemberData(nameof(TestData.CreateValidEditCommentsTestData), MemberType = typeof(TestData))]
    public async Task EditComment_ShouldUpdateContent(EditCommentCommand command)
    {
        var handler = new EditCommentCommandHandler(_userService.Object, _commentsRepository);

        await handler.Handle(command, new CancellationToken());

        var updated = await _commentsRepository.GetByIdAsync(new CommentId(command.CommentId));
        updated.Content.ShouldBe(command.Content);
    }

    [Theory]
    [MemberData(nameof(TestData.CreateInvalidEditCommentsTestData), MemberType = typeof(TestData))]
    public async Task EditComment_ShouldThrow_WhenInvalid(EditCommentCommand command, Type expectedException)
    {
        var handler = new EditCommentCommandHandler(_userService.Object, _commentsRepository);

        var act = () => handler.Handle(command, new CancellationToken());
        await Should.ThrowAsync(act, expectedException);
    }

    [Theory]
    [MemberData(nameof(TestData.CreateValidDeleteCommentsTestData), MemberType = typeof(TestData))]
    public async Task DeleteComment_ShouldRemoveComment(DeleteCommentCommand command)
    {
        var handler = new DeleteCommentCommandHandler(_userService.Object, _commentsRepository);

        await handler.Handle(command, new CancellationToken());

        var deleted = await _commentsRepository.GetByIdAsync(command.CommentId);
        deleted.ShouldBeNull();
    }

    [Theory]
    [MemberData(nameof(TestData.CreateInvalidDeleteCommentsTestData), MemberType = typeof(TestData))]
    public async Task DeleteComment_ShouldThrow_WhenInvalid(DeleteCommentCommand command, Type expectedException)
    {
        if (command.CommentId.Value == 3)
        {
            _userService.Setup(x => x.UserId).Returns("99");
        }

        var handler = new DeleteCommentCommandHandler(_userService.Object, _commentsRepository);

        var act = () => handler.Handle(command, new CancellationToken());
        await Should.ThrowAsync(act, expectedException);
    }
}