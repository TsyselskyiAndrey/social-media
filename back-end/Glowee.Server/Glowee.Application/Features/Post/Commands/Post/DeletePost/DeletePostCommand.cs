using Glowee.Domain.Entities.Posts;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Post.DeletePost;

public record DeletePostCommand(PostId PostId) : IRequest;