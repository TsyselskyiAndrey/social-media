using System.Collections;
using Glowee.Domain.Entities.Posts;
using MediatR;

namespace Glowee.Application.Features.Comment.Queries;

public record GetPostCommentsQuery(PostId PostId) : IRequest<IEnumerable<CommentDto>>;