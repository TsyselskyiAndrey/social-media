using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public record GetSavedPostsQuery() : IRequest<IEnumerable<Domain.Entities.Posts.Post>>;