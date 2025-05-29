using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.MappingProfiles;

public class PostMapper : IPostMapper
{
    private readonly IThumbnailStorageService _thumbnailStorageService;
    private readonly IPostMediaStorageService _postMediaStorageService;
    private readonly IProfileImageStorageService _profileImageStorageService;

    public PostMapper(IThumbnailStorageService thumbnailStorageService
        , IPostMediaStorageService postMediaStorageService, IProfileImageStorageService profileImageStorageService)
    {
        _thumbnailStorageService = thumbnailStorageService;
        _postMediaStorageService = postMediaStorageService;
        _profileImageStorageService = profileImageStorageService;
    }

    public PostDto MapPostToPostDto(Post post, UserId? currentUserId)
    {
        var dto = new PostDto
        {
            Id = post.Id.Value,
            AuthorName = post.User.UserName,
            Caption = post.Caption,
            PostType = post.PostType.Name,
            Likes = post.LikedPosts.Count,
            Views = post.Histories.Count,
            IsLiked = post.LikedPosts.Any(x => x.UserId == currentUserId),
            IsSaved = post.SavedPosts.Any(x => x.UserId == currentUserId),
            IsUninteresting = post.UninterestingPosts.Any(x => x.UserId == currentUserId),
            Tags = post.Tags.Select(x => x.Name).ToList(),
            PostMedias = new List<PostMediaDto>()
        };

        if (post.User.ProfileImagePath != null)
        {
            dto.AuthorIconUrl = _profileImageStorageService.GetProfileImageUrl(post.User.ProfileImagePath);
        }

        foreach (var media in post.PostMedias)
        {
            dto.PostMedias.Add(new PostMediaDto
            {
                Id = media.Id.Value,
                MediaUrl = _postMediaStorageService.GetPostMediaUrl(media.MediaPath),
                ThumbnailUrl = media.ThumbnailPath != null
                    ? _thumbnailStorageService.GetThumbnailUrl(media.ThumbnailPath)
                    : null,
                PostMediaType = media.PostMediaType.Name,
                Duration = media.Duration,
                Format = media.Format,
                Size = media.Size,
                IsUploaded = media.IsUploaded,
                Position = media.Position
            });
        }

        return dto;
    }
}