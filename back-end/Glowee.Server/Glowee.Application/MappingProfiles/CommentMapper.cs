using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.Queries.Comments;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.MappingProfiles
{
    public class CommentMapper : ICommentMapper
    {
        private readonly IProfileImageStorageService _profileImageStorageService;

        public CommentMapper(IProfileImageStorageService profileImageStorageService)
        {
            _profileImageStorageService = profileImageStorageService;
        }

        public CommentDto MapCommentToCommentDto(Comment comment, UserId? currentUserId = null)
        {
            return new CommentDto
            {
                Id = comment.Id.Value,
                Content = comment.Content,
                Author = new CommentUserDto
                {
                    Id = comment.UserId.Value,
                    UserName = comment.User.UserName,
                    ProfileImageUrl = comment.User.ProfileImagePath != null
                            ? _profileImageStorageService.GetProfileImageUrl(comment.User.ProfileImagePath)
                            : null
                },
                IsLiked = currentUserId is not null &&
                      comment.CommentStatuses.Any(x => x.UserId == currentUserId),
                Likes = comment.CommentStatuses.Count
            };
        }
    }
}
