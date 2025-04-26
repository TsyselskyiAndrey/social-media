using Glowee.Domain.Entities.Comments;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Comment.DeleteComment;

public record DeleteCommentCommand(CommentId CommentId) : IRequest;