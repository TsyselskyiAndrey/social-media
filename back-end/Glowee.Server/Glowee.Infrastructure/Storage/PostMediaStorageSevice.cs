using Glowee.Application.Contracts.Storage;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.Posts;

namespace Glowee.Infrastructure.Storage
{
    public class PostMediaStorageSevice : IPostMediaStorageService
    {
        private readonly IBlobStorageService _blobStorageService;

        public PostMediaStorageSevice(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public string GetPostMediaUrl(string blobName)
        {
            return _blobStorageService.GetBlobUrl(BlobContainerType.PostMedia, blobName);
        }

        public async Task RemovePostMediaAsync(string blobName)
        {
            await _blobStorageService.RemoveBlobAsync(BlobContainerType.PostMedia, blobName);
        }

        public async Task<string> UploadPostMediaAsync(Stream stream, string fileName, PostId postId, string? originalBlobName = null)
        {
            return await _blobStorageService.UploadBlob(BlobContainerType.PostMedia, stream, fileName, postId.Value.ToString(), originalBlobName);
        }
    }
}
