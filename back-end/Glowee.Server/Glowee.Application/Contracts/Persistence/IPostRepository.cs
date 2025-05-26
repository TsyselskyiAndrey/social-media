using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.Users;


namespace Glowee.Application.Contracts.Persistence;

public interface IPostRepository : IGenericRepository<Post, PostId>
{
    Task DeleteByUserIdAsync(UserId userId);

    void PostExists(PostId id);

    Task<IEnumerable<Post>> GetIncludedPosts();

    Task CreatePostWithPostMediaAsync(Post post,
        IEnumerable<(Stream stream, string fileName, long size)> medias,
        Stream? thumbnailStream,
        string? thumbnailFileName,
        IEnumerable<Tag> tags,
        PostTypeId postTypeId);
}