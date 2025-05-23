using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Likes;

public record LikeCommand(PostId PostId) : IRequest<bool>;
