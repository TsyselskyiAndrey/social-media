using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetPostsQuery : IRequest<IEnumerable<PostDto>>
{
    public string? PostTitle { get; set; } = String.Empty;
    public int PostsAmount { get; set; } = 20;
    public long? LastPostId { get; set; } = null;
    public List<TagId>? Tags { get; set; } = null;
    public long? UserId { get; set; } = null;
}