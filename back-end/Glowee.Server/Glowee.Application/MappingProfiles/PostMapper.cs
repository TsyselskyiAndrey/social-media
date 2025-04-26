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

    public PostMapper(IThumbnailStorageService thumbnailStorageService
        , IPostMediaStorageService postMediaStorageService)
    {
        _thumbnailStorageService = thumbnailStorageService;
        _postMediaStorageService = postMediaStorageService;
    }

    public PostDto MapPostToPostDtoAsync(Post post, UserId? currentUserId)
    {
        var dto = new PostDto
        {
            Id = post.Id.Value,
            UserId = post.UserId.Value,
            Caption = post.Caption,
            PostType = post.PostType.Name,
            Likes = post.LikedPosts.Count,
            Views = post.Histories.Count,
            IsLiked = post.LikedPosts.Any(x => x.UserId == currentUserId),
            IsSaved = post.SavedPosts.Any(x => x.UserId == currentUserId),
            IsUninteresting = post.UninterestingPosts.Any(x => x.UserId == currentUserId),
            Tags = post.Tags.Select(x => x.Name).ToList(),
            PostMediaDtos = new List<PostMediaDto>()
        };

        foreach (var media in post.PostMedias)
        {
            dto.PostMediaDtos.Add(new PostMediaDto
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