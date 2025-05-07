using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User, UserId>, IUserRepository
    {
        IFollowRepository _followRepository;
        ICommentsRepository _commentsRepository;
        IMessageRepository _messageRepository;
        IPostRepository _postRepository;
        INotificationRepository _notificationRepository;
        IReportRepository _reportRepository;
        IRequestRepository _requestRepository;

        public UserRepository(SqlDbContext context, IFollowRepository followRepository, ICommentsRepository commentsRepository, IMessageRepository messageRepository, IPostRepository postRepository, INotificationRepository notificationRepository, IReportRepository reportRepository, IRequestRepository requestRepository) : base(context)
        {
            _followRepository = followRepository;
            _commentsRepository = commentsRepository;
            _messageRepository = messageRepository;
            _postRepository = postRepository;
            _notificationRepository = notificationRepository;
            _reportRepository = reportRepository;
            _requestRepository = requestRepository;
        }

        /// Before we remove a user we need to remove their followings, comments, posts(to remove comments below the posts), 
        /// notifications and messages to prevent the cascade error.
        public override async Task DeleteAsync(UserId id)
        {
            await _commentsRepository.DeleteByUserIdAsync(id);
            await _postRepository.DeleteByUserIdAsync(id);
            await _notificationRepository.DeleteByUserIdAsync(id);
            await _followRepository.DeleteByFollowerIdAsync(id);
            await _messageRepository.DeleteBySenderIdAsync(id);
            await _reportRepository.DeleteByCompliantIdAsync(id);
            await _requestRepository.DeleteBySenderIdAsync(id);
            await base.DeleteAsync(id);
        }

        public async Task UpdateProfileImageAsync(UserId userId, string imagePath)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.ProfileImagePath = imagePath;
            await _context.SaveChangesAsync();
        }
    }
}
