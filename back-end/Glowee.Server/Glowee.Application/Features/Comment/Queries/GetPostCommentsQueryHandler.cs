using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Comment.Queries;

public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _commentsRepository;
    private readonly IPostRepository _postRepository;
    private readonly ICommentMapper _commentMapper;
    private readonly IUserService _userService;

    public GetPostCommentsQueryHandler(ICommentRepository commentsRepository
        , IPostRepository postRepository, ICommentMapper commentMapper
        , IUserService userService)
    {
        _commentsRepository = commentsRepository;
        _postRepository = postRepository;
        _commentMapper = commentMapper;
        _userService = userService;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        _postRepository.PostExists(request.PostId);

        var postComments = await _commentsRepository.GetIncludedPostComments(request.PostId);

        var currentUserId = _userService.UserId != null
            ? new UserId(Convert.ToInt64(_userService.UserId))
            : null;

        var result = postComments
            .Select(x => _commentMapper.MapCommentToCommentDto(x, currentUserId));

        return result;
    }
}