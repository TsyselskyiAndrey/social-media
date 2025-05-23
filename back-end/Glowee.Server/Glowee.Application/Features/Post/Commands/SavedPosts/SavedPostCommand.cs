using Glowee.Domain.Entities.Posts;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.SavedPosts;

public record SavedPostCommand(PostId PostId) : IRequest<bool>;