using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.CommentStatuses;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Comment.Commands.LikeComment
{
    public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentStatusRepository _commentStatusRepository;

        public LikeCommentCommandHandler(IUserService userService, ICommentRepository commentRepository, ICommentStatusRepository commentStatusRepository)
        {
            _userService = userService;
            _commentRepository = commentRepository;
            _commentStatusRepository = commentStatusRepository;
        }

        public async Task<bool> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userService.UserId))
                throw new UnauthorizedAccessException("User must be authenticated to like a comment.");

            var userId = long.Parse(_userService.UserId);

            _commentRepository.CommentExists(request.CommentId);

            var commentStatuses = await _commentStatusRepository.GetAsync();
            var commentLike = commentStatuses.FirstOrDefault(x => x.CommentId == request.CommentId && x.UserId.Value == userId);

            if (commentLike == null)
            {
                await _commentStatusRepository.CreateAsync(new CommentStatus()
                {
                    CommentId = request.CommentId,
                    UserId = new UserId(userId),
                    IsLiked = true
                });

                return true;
            }
            await _commentStatusRepository.DeleteAsync(commentLike.Id);

            return false;
        }
    }
}
