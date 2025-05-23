using System.Text.RegularExpressions;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.PostMediaTypes;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Tags;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class PostRepository : GenericRepository<Post, PostId>, IPostRepository
{
    private readonly IBlobStorageService _blobStorageService;

    public PostRepository(SqlDbContext connection, IBlobStorageService blobStorageService) : base(connection)
    {
        _blobStorageService = blobStorageService;
    }
    public void PostExists(PostId id)
    {
        if (!_context.Posts.Any(p => p.Id == id))
        {
            throw new NotFoundException($"Post with id {id} does not exist");
        }
    }

    public async Task<IEnumerable<Post>> GetIncludedPosts()
    {
        return await _context.Posts
                .Include(x => x.LikedPosts)
                .Include(x => x.Tags)
                .Include(x => x.PostType)
                .Include(x => x.UninterestingPosts)
                .Include(x => x.PostMedias)
                    .ThenInclude(x => x.PostMediaType)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task CreatePostWithPostMediaAsync(
        Post post,
        IEnumerable<(Stream stream, string fileName, long size)> medias,
        Stream? thumbnailStream,
        string? thumbnailFileName,
        IEnumerable<Tag> tags,
        PostTypeId postTypeId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        var uploadedFiles = new List<(BlobContainerType containerType, string blobName)>();

        try
        {
            post.PostTypeId = postTypeId;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            var postMedias = new List<PostMedia>();
            int position = 1;

            foreach (var (stream, fileName, size) in medias)
            {
                var ext = Path.GetExtension(fileName).ToLower();
                var containerType = BlobContainerType.PostMedia;
                var blobPath = await _blobStorageService.UploadBlob(containerType, stream, fileName, post.Id.ToString());

                uploadedFiles.Add((containerType, Path.Combine(post.Id.ToString(), fileName)));

                var mediaTypeId = Regex.IsMatch(ext, @"\.(jpg|jpeg|png|gif)$", RegexOptions.IgnoreCase)
                    ? new PostMediaTypeId(1)
                    : new PostMediaTypeId(2);

                postMedias.Add(new PostMedia
                {
                    PostId = post.Id,
                    MediaPath = blobPath,
                    Format = ext.Trim('.'),
                    Size = size,
                    Position = position++,
                    IsUploaded = true,
                    PostMediaTypeId = mediaTypeId
                });
            }

            if (postTypeId.Value == 2)
            {
                var uniqueThumbName = "thumb_" + Guid.NewGuid();
                var containerType = BlobContainerType.PostMedia;
                var thumbPath = await _blobStorageService.UploadBlob(containerType, thumbnailStream!, uniqueThumbName, post.Id.ToString());

                uploadedFiles.Add((containerType, Path.Combine(post.Id.ToString(), uniqueThumbName)));

                var firstVideo = postMedias.FirstOrDefault(m => m.PostMediaTypeId.Value == 2);
                if (firstVideo != null)
                    firstVideo.ThumbnailPath = thumbPath;
            }

            await _context.PostMedias.AddRangeAsync(postMedias);
            
            foreach (var media in postMedias)
                post.PostMedias.Add(media);

            foreach (var tag in tags)
                post.Tags.Add(tag);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();

            foreach (var (containerType, blobName) in uploadedFiles)
            {
                await _blobStorageService.RemoveBlobAsync(containerType, blobName);
            }

            throw;
        }
    }
}