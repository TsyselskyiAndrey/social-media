using Glowee.Domain.Entities.Comments;
using MediatR;

namespace Glowee.Application.Features.Comment.Commands.DeleteComment;

public record DeleteCommentCommand(CommentId CommentId) : IRequest;