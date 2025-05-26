using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetPostsQuery : IRequest<IEnumerable<PostDto>>
{
    public int PostsAmount { get; set; } = 20;
    public PostId? LastPostId { get; set; } = null;
    public List<TagId>? Tags { get; set; } = null;
    public UserId? UserId { get; set; } = null;
}