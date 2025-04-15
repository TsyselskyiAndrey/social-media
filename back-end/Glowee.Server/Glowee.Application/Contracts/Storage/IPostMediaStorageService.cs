using Glowee.Domain.Entities.Posts;

namespace Glowee.Application.Contracts.Storage
{
    public interface IPostMediaStorageService
    {
        Task<string> UploadPostMediaAsync(Stream stream, string fileName, PostId postId, string? originalBlobName = null);
        string GetPostMediaUrl(string blobName);
        Task RemovePostMediaAsync(string blobName);
    }
}
