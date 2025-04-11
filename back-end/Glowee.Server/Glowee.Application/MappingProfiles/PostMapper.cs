using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.Features.Post.Queries.Comments;
using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.UninterestingPosts;

namespace Glowee.Application.MappingProfiles;

public static class PostMapper
{
    public static PostDto MapPostToPostDto(this Post post)
    {
        return new PostDto
        {
            Id = post.Id.Value,
            UserId = post.UserId.Value,
            Caption = post.Caption,
            PostType = post.PostType.Name,
            Likes = post.LikedPosts.Count,
            Views = post.Histories.Count,
            IsLiked = post.LikedPosts.Any(x => x.UserId == post.UserId),
            IsSaved = post.SavedPosts.Any(x => x.UserId == post.UserId),
            IsUninteresting = post.UninterestingPosts.Any(x => x.UserId == post.UserId),
            Tags = post.Tags.Select(x => x.Name).ToList(),
            PostMediaDtos = post.PostMedias.Select(x => new PostMediaDto()
            {
                Id = x.Id.Value,
                MediaUrl = x.MediaUrl,
                PostMediaType = x.PostMediaType.Name,
                ThumbnailUrl = x.ThumbnailUrl,
                Duration = x.Duration,
                Format = x.Format,
                Size = x.Size,
                IsUploaded = x.IsUploaded,
                Position = x.Position,
            }).ToList(),
        };
    }

    public static CommentDto MapCommentToCommentDto(this Comment comment)
    {
        var result = new CommentDto()
        {
            Id = comment.Id.Value,
            Content = comment.Content,
            Author = new CommentUserDto()
            {
                Id = comment.UserId.Value,
                UserName = comment.User.UserName,
                ProfileImageUrl = comment.User.ProfileImageUrl,
            },
            IsLiked = comment.CommentStatuses.Any(x => x.UserId == comment.UserId),
            Likes = comment.CommentStatuses.Count(x => x.Comment.Id == comment.Id),
        };
        
        return result;
    }   
}