using Glowee.Domain.Entities.Posts;

namespace Glowee.Application.Contracts.Storage
{
    public interface IThumbnailStorageService
    {
        Task<string> UploadThumbnailAsync(Stream stream, string fileName, PostId postId, string? originalBlobName = null);
        string GetThumbnailUrl(string blobName);
        Task RemoveThumbnailAsync(string blobName);
    }
}
