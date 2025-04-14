using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Queries.Posts;
using Glowee.Application.MappingProfiles;
using Glowee.Domain.Entities.Posts;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Comments;

public class GetPostCommentsQueryHandler: IRequestHandler<GetPostCommentsQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IPostRepository _postRepository;

    public GetPostCommentsQueryHandler(ICommentsRepository commentsRepository, IPostRepository postRepository)
    {
        _commentsRepository = commentsRepository;
        _postRepository = postRepository;
    }
    
    public async Task<IEnumerable<CommentDto>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        _postRepository.PostExists(request.PostId);
        
        var postComments = await _commentsRepository.GetIncludedPostComments(request.PostId);

        var result = postComments.Select(x => x.MapCommentToCommentDto());
        
        return result;
    }
}