using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.SavedPosts;

public record GetSavedPostsQuery(UserId UserId) : IRequest<IEnumerable<PostDto>>;